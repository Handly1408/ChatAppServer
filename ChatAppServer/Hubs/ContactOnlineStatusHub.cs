using ChatAppServer.Constants;
using ChatAppServer.Model;
using ChatAppServer.Util;
using Google.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ChatAppServer.Hubs
{
    [Authorize]
    public class ContactOnlineStatusHub: Hub
    {
        public override async Task OnConnectedAsync()
        {
            (string? id,_) = ClientsUtil.AddChatContactStatusClient(Context,GetType().Name);

            if (string.IsNullOrEmpty(id)) return;
            var newChatStatus = new ContactChatStatusModel(id, true);
            await Clients.Groups(id).SendAsync(ServerConstants.ON_CLIENT_STATUS_CHANGED,newChatStatus);
            await UserChatStatusUtil.SaveStatusAsync(id,newChatStatus);
             
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string id = CliemsUtil.GetUserId(Context)!;
            if (id.IsNullOrEmpty()) return;
             
            var connectionId = Context.ConnectionId;
            Console.WriteLine($"\nOn Connected {connectionId}");
            var newChatStatus = new ContactChatStatusModel(id, false);
            await Clients.Groups(id).SendAsync(ServerConstants.ON_CLIENT_STATUS_CHANGED, newChatStatus);
            Console.Write($"\n On client status changed -> client connection id: {newChatStatus.ContactId} -> new status: {newChatStatus.IsActive}");
            await UserChatStatusUtil.SaveStatusAsync(id, newChatStatus);
            ClientsUtil.RemoveChatContactStatusClient(Context, GetType().Name);
            return;
        }
        /// <summary>
        /// Call this method on client side to get chat clients status
        /// </summary>
        /// <param name="contactId">Friend,Contact</param>
        /// <returns></returns>
        public async Task SubscribeOnContactStatusChanged(string contactId)
        {
            if (contactId.IsNullOrEmpty()) return;
            await Groups.AddToGroupAsync(Context.ConnectionId, contactId);
            await Clients.Caller.SendAsync(ServerConstants.ON_CLIENT_STATUS_CHANGED, await UserChatStatusUtil.GetStatusAsync(contactId));
        }
        public async Task NotifyMyChatStatus(ContactChatStatusModel contactChatStatusModel)
        {

            string id = CliemsUtil.GetUserId(Context)!;
 
            await Clients.Groups(id).SendAsync(ServerConstants.ON_CLIENT_STATUS_CHANGED, contactChatStatusModel);
            Console.Write($"\nSend new online status: is active -> {contactChatStatusModel.IsActive} -> FB: {id}");

        }
    }
}
