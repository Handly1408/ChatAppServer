using Google.Api;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppServer.Util
{
    public static class CliemsUtil
    {
        public static string? GetUserId(this HubCallerContext context)
        {
            var resultId = context?.User?.Claims.FirstOrDefault();
            return resultId?.Value.ToString();
        }
        public static string? GetUserId(this HubCallerContext context, string userId)
        {
            if (userId == null) return null;
            var resultId = context?.User?.Claims.FirstOrDefault(x => x.Equals(userId));
            return resultId?.ToString();
        }

    }
}
