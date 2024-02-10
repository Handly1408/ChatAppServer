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

     
        public async Task SendSignalCallToUser(DataModel dataModel)
        {
            
            try
            {
                ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.TargetId, out string? receiverConnectionId);
                switch (dataModel.GetDataModelTypeEnum())   
                {
                    
                    case DataModelTypeEnum.INCOMING_CALL://FOR SENDER OF THE CALL

                        if (CallUtil.ActiveCalls.TryGetValue(dataModel.TargetId,out var existCall))
                        {
                            dataModel.DataModelTypeEnum = EnumUtil.GetStrFromEnum(DataModelTypeEnum.CALL_RECEIVER_BUSY)!;

                            await Clients.Caller.SendAsync("onSignalReceivedCallBack", dataModel);
                            Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the sender: Name {dataModel.SenderName} FB: {dataModel.SenderId}");

                            //Second incoming call while a current existing call
                            dataModel.DataModelTypeEnum = EnumUtil.GetStrFromEnum(DataModelTypeEnum.SECOND_INCOMING_CALL)!;

                            break;
                        }
                       
                        CallUtil.InitiateCall(async (o, a)=>{
                            //TODO: oPTIMIZE FOR GROUP CALLS
                             ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.SenderId, out string? senderConnectionId);
                             
                            dataModel.DataModelTypeEnum = EnumUtil.GetStrFromEnum(DataModelTypeEnum.CALL_TIMEOUT)!;
                            if (Utils.CheckStrForNull(dataModel.DataModelTypeEnum))
                            {
                                throw new NullReferenceException("$Can not send null command");
                            }
                            if (!Utils.CheckStrForNull(senderConnectionId) && CallUtil.ActiveCalls.TryGetValue(dataModel.SenderId, out var incomingCall))
                            {
                                //TODO: TEST REQUAE
                                //await incomingCall.Clients.Caller.SendAsync("onSignalReceivedCallBack", dataModel);

                                //INSTEAD
                                //We gonna call our self but through the Client.Client
                                //instead of Clients.Caller and pass (our self id)  

                                await incomingCall.Clients.Client(senderConnectionId!).SendAsync("onSignalReceivedCallBack", dataModel);
                                Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the sender: Name {dataModel.SenderName} FB: {dataModel.SenderId}");

                                //TODO:OPTIMIZATION OF THE TRAFIC HERE SEND MESSAGE TO THE RECEIVER ALSO
                                ClientsUtil.AuthorizedWebRTCClients.TryGetValue(dataModel.TargetId, out string? receiverConnectionId);
                                //Send Call time out to the receiver
                                await incomingCall.Clients.Client(receiverConnectionId!).SendAsync("onSignalReceivedCallBack", dataModel);
                                Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the Reсeiver FB: {dataModel.TargetId}");


                            }

                            CallUtil.ActiveCalls.Remove(dataModel.SenderId);//Remove the active call

                            //-----------------------------------------------------

                        }
                        ,callerCnnectionId: dataModel.SenderId,Clients);
                        break;
                    case DataModelTypeEnum.CALL_ACCEPTED:
                        CallUtil.ActiveCalls.TryGetValue(dataModel.TargetId,out var callAccepted);
                        if (!Utils.CheckForNull(callAccepted))
                        {
                            callAccepted.StopTimer();
                            //activeCalls.Remove(dataModel.TargetId);//Remove the active call

                        }

                        break;
                    case DataModelTypeEnum.CALL_REJECTED_OR_END:
                        //Seens we dont know who send this status we gonna find the owner of the status
                        var call = CallUtil.FindActiveCallInfoModelByCallOwner(dataModel);
                        if (!Utils.CheckForNull(call))
                        {
                            call.StopTimer();
                            CallUtil.ActiveCalls.Remove(dataModel.TargetId);//Remove the active call

                        }

                        break;

                }

                if (!Utils.CheckStrForNull(receiverConnectionId))
                {      //Call Rejected when target client was not in online
                       // should be never happend just in case

                    if (dataModel.GetDataModelTypeEnum().Equals(DataModelTypeEnum.CALL_REJECTED_OR_END))
                    {
                         return;
                     }
                      //--------------------------------------------------------------------------------
                     
                    //Offline send
                    string? notificationToken = await FireBaseDbService.Instance.GetNotificationTokenAsync(dataModel.TargetId);
                    await FireBaseAdminService.Instance.SendIncomingCallAsync(notificationToken, dataModel);
                    return;
                }

                //await Clients.Caller.SendAsync("onSignalReceivedCallBack",dataModel);
                await Clients.Client(receiverConnectionId!).SendAsync("onSignalReceivedCallBack", dataModel);
                Console.Write($"\nSignal send -> {dataModel.DataModelTypeEnum} to the receiver FB: {dataModel.TargetId}");

            }
            catch (Exception ex)
            {

                Console.Write($"\n{ex.Message}");

            }
        }
    }
}
