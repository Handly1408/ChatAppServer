using ChatAppServer.Constants;
using ChatAppServer.Model;
using ChatAppServer.Services;
using ChatAppServer.Util;
using DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
 

namespace ChatAppServer.Hubs
{
    [Authorize]
    public class MessangerHub : Hub
    {
        private ChatUserNotificationTokenEntityRepository ChatUserNotificationTokenEntityRepository { get; set; } = new();
        public override Task OnConnectedAsync()
        {
            TimeUtil.GetCurrentTimeMilliseconds();
            TimeUtil.GetRegionTime();
            ClientsUtil.AddMessegingClient(Context, GetType().Name);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.Write($"\nOn Disconnected");
            

            ClientsUtil.RemoveMessegingClient(Context,GetType().Name);
 
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(SendMessageArgs sendMessageArgs, string receiverId)
        {
            Console.Write($"\nMessage received: {sendMessageArgs.MessageJson} :Message status {sendMessageArgs.MessageEventTypeEnum}");
           
            await Clients.Caller.SendAsync(ServerConstants.ON_SEND_MESSAGE_CALL_BACK, sendMessageArgs);
           

            ClientsUtil.AuthorizedMessegingClients.TryGetValue(receiverId, out string? receiverConnectionId);
            if (receiverConnectionId.IsNullOrEmpty() )
            {
                // Offline send
                string? notificationToken = await FireBaseDbService.Instance.GetNotificationTokenAsync(receiverId);
                await FireBaseAdminService.SendMessageAsync(notificationToken, sendMessageArgs);
                return;
            }
            Console.Write($"\nMessage send to receiver id: {receiverConnectionId}");
            //Single client message send
            if (sendMessageArgs.ExtraReceiversId.IsNullOrEmpty()) {
                   
                await Clients.Client(receiverConnectionId!).SendAsync(ServerConstants.ON_RECEIVE_MESSAGE_CALL_BACK, sendMessageArgs);
                return ;

            }
            //Multiple clients message send (Group)
            foreach (var extraReceiverId in sendMessageArgs.ExtraReceiversId)
            {
                ClientsUtil.AuthorizedMessegingClients.TryGetValue(extraReceiverId, out string? extraReceiverConnectionId);
                //TODO: Offline group clients message send (notification)

                await Clients.Client(extraReceiverConnectionId!).SendAsync(ServerConstants.ON_RECEIVE_MESSAGE_CALL_BACK, sendMessageArgs);
                

            }



            //await Clients.Users(receiverConnectionId).SendAsync("onReceiveMessage", sendMessageArgs);
        }

     
        
    }
}
