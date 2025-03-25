using ChatAppServer.Model;
using Microsoft.AspNetCore.SignalR;
using System.Timers;

namespace ChatAppServer.Util
{
    public class CallUtil

    {
        public static Dictionary<string, CallInfoModel> ActiveCalls { get; }= new (){ };

        public static void InitiateCall(ElapsedEventHandler timeElapsed,string callId,
            IHubCallerClients clients)
        {
            // Сохранение информации о звонке
            ActiveCalls.Add(callId, new CallInfoModel(timeElapsed,
                callId, clients));
            // Отправка уведомления клиенту

        }


        public static CallInfoModel? FindActiveCallInfoModelByCallOwner(SignalTargetDataModel dataModel) {
            CallInfoModel? timer1, timer2 = null;

             ActiveCalls.TryGetValue(dataModel.TargetId, out timer1);
            if (timer1 == null)
                ActiveCalls.TryGetValue(dataModel.SenderId, out timer2);

            return timer1 != null ? timer1 : timer2;
        }
        public static void RemoveCallIfOwner(string callOwner)
        {
             

            ActiveCalls.TryGetValue(callOwner, out var call);
            if (call != null)
            {
                call.StopTimer();
                ActiveCalls.Remove(callOwner);
            }
               

             
        }
    }
}
