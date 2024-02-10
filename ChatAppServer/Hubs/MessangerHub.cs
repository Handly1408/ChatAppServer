using ChatAppServer.Constants;
using ChatAppServer.Model;
using ChatAppServer.Services;
using ChatAppServer.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
 

namespace ChatAppServer.Hubs
{
    [Authorize]
    public class MessangerHub : Hub
    {
 
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
                //TODO:Offline send
                string? notificationToken = await FireBaseDbService.Instance.GetNotificationTokenAsync(receiverId);
                await FireBaseAdminService.Instance.SendMessageAsync(notificationToken, sendMessageArgs);
                return;
            }
            Console.Write($"\nMessage send to receiver id: {receiverConnectionId}");

            await Clients.Client(receiverConnectionId!).SendAsync(ServerConstants.ON_RECEIVE_MESSAGE_CALL_BACK, sendMessageArgs);


            //await Clients.Users(receiverConnectionId).SendAsync("onReceiveMessage", sendMessageArgs);
        }

     
        
    }
}
