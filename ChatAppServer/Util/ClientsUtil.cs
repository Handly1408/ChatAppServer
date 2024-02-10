using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;

namespace ChatAppServer.Util
{
    internal static class ClientsUtil
    {
        public static ConcurrentDictionary<string, string> AuthorizedMessegingClients { get; private set; } = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> AuthorizedChatContactStatusClients { get; private set; } = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> AuthorizedWebRTCClients { get; private set; } = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// Add new client to list
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hubName"></param>
        /// <returns>return User FB id and User connection id</returns>
        public static (string?, string?) AddMessegingClient(HubCallerContext context, string? hubName = null)
        {
            return UpdateList(AuthorizedMessegingClients, context, hubName);

        }
        public static void RemoveMessegingClient(HubCallerContext context,string? hubName = null)
        {
             RemoveFromList(AuthorizedMessegingClients, context, hubName);
        }
        public static (string?, string?) AddChatContactStatusClient(HubCallerContext context, string? hubName = null)
        { 
          return UpdateList(AuthorizedChatContactStatusClients,context,hubName); 
        }
        public static void RemoveChatContactStatusClient(HubCallerContext context, string? hubName = null)
        {
           RemoveFromList(AuthorizedChatContactStatusClients, context, hubName);
        }

        
        public static (string?, string?) AddWebRTCClient(HubCallerContext context, string? hubName = null)
        { 
            return UpdateList(AuthorizedWebRTCClients, context, hubName);
        }
        public static void RemoveWebRTCClient(HubCallerContext context, string? hubName = null)
        {
            RemoveFromList(AuthorizedWebRTCClients, context, hubName);
        }
        private static (string?, string?) UpdateList(ConcurrentDictionary<string, string> toUpdate, HubCallerContext context, string? hubName = null)
        {
            if (context == null) return (null, null);
            string id = CliemsUtil.GetUserId(context)!;
            if (id.IsNullOrEmpty()) return (null, null);
            string connectionId = context.ConnectionId;
            toUpdate.AddOrUpdate(id, connectionId, (k, v) => v);
            Console.WriteLine($"Client connected to: {hubName ?? "????"} connection id: {connectionId} FB id:{id}");
            return (id, connectionId);

        }
        private static void RemoveFromList(ConcurrentDictionary<string, string> toUpdate, HubCallerContext context, string? hubName = null)
        {
            if (context == null) return;
            string id = CliemsUtil.GetUserId(context)!;
            if (id.IsNullOrEmpty()) return;
             
            toUpdate.Remove(id,out var remove);
            Console.WriteLine($"Client removed from: {hubName ?? "????"} connection id: {remove} FB id: {id}");
            

        }
    }
    }
