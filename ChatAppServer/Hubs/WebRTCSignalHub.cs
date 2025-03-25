using ChatAppServer.Model;
using ChatAppServer.Services;
using ChatAppServer.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.Hubs
{
    [Authorize]
    internal class WebRTCSignalHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            ClientsUtil.AddWebRTCClient(Context, GetType().Name);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)

        {
            string id = CliemsUtil.GetUserId(Context)!;

            ClientsUtil.RemoveWebRTCClient(Context, GetType().Name);
            CallUtil.RemoveCallIfOwner(id);
           

            return base.OnDisconnectedAsync(exception);
        }
       


        public async Task SendSignalCallToUser(SignalTargetDataModel dataModel)
        {
            
            try
            {
                CallInfoModel callInfoModel;
                ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.TargetId, out string? receiverConnectionId);
                //Call statuses
                switch (dataModel.GetDataModelTypeEnum())   
                {
                    
                   
                    case DataModelTypeEnum.OUTGOING_CALL:

                        if (CallUtil.ActiveCalls.TryGetValue(dataModel.TargetId, out var existCall))
                        {
                            //TODO:IN APP RECEIVER IS BUSSY STATUS
                            dataModel.DataModelTypeEnum = EnumUtil.GetStrFromEnum(DataModelTypeEnum.CALL_RECEIVER_BUSY)!;

                            await Clients.Caller.SendAsync("onSignalReceivedCallBack", dataModel);
                            Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the sender: Name {dataModel.SenderName} FB: {dataModel.SenderId}");

                            //Second incoming call while a current existing call
                            // dataModel.DataModelTypeEnum = EnumUtil.GetStrFromEnum(DataModelTypeEnum.SECOND_INCOMING_CALL)!;
                           await SendCallToReceiver(dataModel, DataModelTypeEnum.ANOTHER_INCOMING_CALL, receiverConnectionId,Clients);

                           break;

                        }
                        else
                        {
                            await SendCallToReceiver(dataModel, DataModelTypeEnum.INCOMING_CALL, receiverConnectionId,Clients);

                        }

                        //Start timer for cancel the call by time elapsed
                        CallUtil.InitiateCall(async (o, a) =>
                        {
                            //TODO: PTIMIZE FOR GROUP CALLS
                          ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.SenderId, out string? senderConnectionId);
                          ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.TargetId, out string? receiverConnectionId);

                            var callEvent = DataModelTypeEnum.CALL_TIMEOUT; 
                            dataModel.DataModelTypeEnum = EnumUtil.GetStrFromEnum(callEvent)!;

                            //Check if the caller is online and call exist (the last just in case)
                            if (!Utils.CheckStrForNull(senderConnectionId) && CallUtil.ActiveCalls.TryGetValue(dataModel.SenderId, out var incomingCall)/*just in case*/)
                            {
                                //Cancel the call by timeout for sender
                                await incomingCall.Clients.Caller.SendAsync("onSignalReceivedCallBack", dataModel);
                                Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the sender: Name {dataModel.SenderName} FB: {dataModel.SenderId}");

                                //TODO:OPTIMIZATION OF THE TRAFIC HERE SEND MESSAGE TO THE RECEIVER ALSO
                                //Send Call time out to the receiver
                                await SendCallToReceiver(dataModel, callEvent, receiverConnectionId,incomingCall.Clients);



                            }

                            CallUtil.ActiveCalls.Remove(dataModel.SenderId);//Remove the active call

                            //-----------------------------------------------------

                        }
                        //----------------------------------------------
                        , callId: dataModel.SenderId, Clients);
                        break;
                    case DataModelTypeEnum.CALL_ACCEPTED:
                        CallUtil.ActiveCalls.TryGetValue(dataModel.TargetId /*in this case this is the call creator sender*/, out callInfoModel);
                        if (!Utils.CheckForNull(callInfoModel))
                        {
                            callInfoModel.StopTimer();
                            //activeCalls.Remove(dataModel.TargetId);//Remove the active call

                        }
                        await SendCallToReceiver(dataModel,DataModelTypeEnum.CALL_ACCEPTED,receiverConnectionId, Clients);
                        break;
                    /*
                       case DataModelTypeEnum.VIDEO_CALL_ACCEPTED:

                          await SendCallToReceiver(dataModel,
                          DataModelTypeEnum.VIDEO_CALL_ACCEPTED,
                          receiverConnectionId, Clients);
                          break;
                     */
                    case DataModelTypeEnum.CALL_REJECTED_OR_END:
                        //Seens we dont know who send this status we gonna find the owner of the status

                        callInfoModel= CallUtil.FindActiveCallInfoModelByCallOwner(dataModel)!;
                        if (!Utils.CheckForNull(callInfoModel))
                        {
                            callInfoModel.StopTimer();
                            CallUtil.ActiveCalls.Remove(callInfoModel.CallerId);//Remove the active call

                        }
                        await SendCallToReceiver(dataModel,DataModelTypeEnum.CALL_REJECTED_OR_END,receiverConnectionId, Clients);

                        return;
                    case DataModelTypeEnum.CONFIRM_CALL:
                        await SendConfirmCall(dataModel);

                        break;
                    //ICE CANDIDATE,OFFER,ANSWER
                    /*
                        case DataModelTypeEnum.OFFER:
                       case DataModelTypeEnum.ANSWER:
                           case DataModelTypeEnum.ICE_CANDIDATE:
                           await SendCallToReceiver(dataModel,
                               dataModel.GetDataModelTypeEnum(),
                               receiverConnectionId, Clients);
                           break;

                     */
                    //ICE CANDIDATE,OFFER,ANSWER etc...
                    default:
                        await SendCallToReceiver(dataModel,
                              dataModel.GetDataModelTypeEnum(),
                              receiverConnectionId, Clients);
                        break;

                }




            }
            catch (Exception ex)
            {

                Console.Write($"\n{ex.Message}");

            }
        }

        public async Task SendConfirmCall(SignalTargetDataModel dataModel)
        {
             ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.TargetId, out string? receiverConnectionId);

             ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.SenderId, out string? callerConnectionId);

            if (!Utils.CheckStrForNull(callerConnectionId) && CallUtil.ActiveCalls.TryGetValue(dataModel.SenderId, out var incomingCallForCheck) /*just in case*/)
            {
                //Cancel the call by timeout for sender

                Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the sender: Name {dataModel.SenderName} FB: {dataModel.SenderId}");

                //TODO:OPTIMIZATION OF THE TRAFIC HERE SEND MESSAGE TO THE RECEIVER ALSO
                //Send Call time out to the receiver
                dataModel.SetDataModelTypeEnum(DataModelTypeEnum.CALL_CONFIRM_SUCCESS);

                await SendCallToReceiver(dataModel, dataModel.GetDataModelTypeEnum(), receiverConnectionId, incomingCallForCheck.Clients);


            }
            else
            {   //No call exist anymore or the caller
                dataModel.SetDataModelTypeEnum(DataModelTypeEnum.CALL_CONFIRM_UNSUCCESSFUL);
                await SendCallToReceiver(dataModel, dataModel.GetDataModelTypeEnum(), receiverConnectionId, Clients);


            }
        }


        private async Task SendCallToReceiver(SignalTargetDataModel dataModel, DataModelTypeEnum callEvent, string? receiverConnectionId,IHubCallerClients clients)
      
            {
            dataModel.DataModelTypeEnum = EnumUtil.GetStrFromEnum(callEvent)!;

            if (Utils.CheckStrForNull(receiverConnectionId))
            {
                 await FireBaseAdminService.Instance.SendIncomingCallAsync(dataModel.TargetId, dataModel);
            }
            else
            {
                
                await clients.Client(receiverConnectionId!).SendAsync("onSignalReceivedCallBack", dataModel);
                Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the receiver FB: {dataModel.TargetId}");

           }
        }
    }
}
