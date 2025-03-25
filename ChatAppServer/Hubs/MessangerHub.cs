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
            (string? userId, _) = ClientsUtil.AddMessegingClient(Context, GetType().Name);
            return base.OnConnectedAsync();
        }



        public override Task OnDisconnectedAsync(Exception? exception)
        {
            ClientsUtil.RemoveMessegingClient(Context,GetType().Name);
 
            return base.OnDisconnectedAsync(exception);
        }
         /// <summary>
         /// Call from the app (Each user will save their own public key)
         /// </summary>
         /// <param name="publicKeyEncode"></param>
         /// <returns></returns>
        public async Task SendPublicKey(string publicKeyEncode)
        {
            string userId = CliemsUtil.GetUserId(Context)!;
            await UserDataUtil.SaveClientPublicKeyAsync(userId,new UserPublicKeyModel(userId,publicKeyEncode));
        }
        public async Task<string> GetContactKey(string contactId)
        {
            UserPublicKeyModel userPublicKeyModel= await UserDataUtil.GetUserPublicKeyAsync(contactId);
            Console.WriteLine($"{nameof(GetContactKey)} -> Contact id->{userPublicKeyModel.UserId} Public key->{userPublicKeyModel.PublicKey}");
            return userPublicKeyModel!.PublicKey==""?"null": userPublicKeyModel!.PublicKey;
 
        }


        public async Task SendMessage(SendMessageArgs sendMessageArgs, string receiverId)
        {
            Console.Write($"\nMessage received: {sendMessageArgs.MessageJson} :Message status {sendMessageArgs.MessageEventTypeEnum}");
           
            await Clients.Caller.SendAsync(ServerConstants.ON_SEND_MESSAGE_CALL_BACK, sendMessageArgs);
           

            ClientsUtil.AuthorizedMessegingClients.TryGetValue(receiverId, out string? receiverConnectionId);
            if (receiverConnectionId.IsNullOrEmpty() )
            {
                //TODO:Offline send
                Console.ForegroundColor = ConsoleColor.Blue;
       
                Console.ResetColor();

                 await FireBaseAdminService.Instance.SendMessageAsync(receiverId, sendMessageArgs);
                return;
            }
            Console.Write($"\nSend message to receiver id: {receiverId}");

             await Clients.Client(receiverConnectionId!).SendAsync(ServerConstants.ON_RECEIVE_MESSAGE_CALL_BACK, sendMessageArgs);


            //await Clients.Users(receiverConnectionId).SendAsync("onReceiveMessage", sendMessageArgs);
        }

     
        
    }
}
