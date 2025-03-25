using ChatAppServer.Constants;
using ChatAppServer.Model;
using ChatAppServer.Util;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ChatAppServer.Services
{
    public class FireBaseAdminService : FirebaseServiceBase<FireBaseAdminService>

    {

        public FireBaseAdminService()
        {
            try
            {
                //var json = Program.App.Configuration.Get<FirebaseCredential>();
                var credential = GoogleCredential.FromFile(FirebaseUtil.GetServiceAccountPath());
              
                var app = FirebaseApp.Create(new AppOptions
                {
                    Credential = credential,
                });

                FirebaseUtil.DeletePathToServiceAccount();
                Initialized = true;
            }
            catch (Exception ex)
            {
                // Обработка ошибок при инициализации Firebase
                Console.WriteLine($"Ошибка при инициализации Firebase: {ex.Message}");
                Initialized = false;
            }
        }
        public async Task<bool> GetUserAsync(string uid)
        {

            if (!FirebaseUtil.CheckAdminInit())
            {
                return false;
            }

            try
            {
                var user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
                return user != null;
            }
            catch (FirebaseAuthException ex)
            {
                // Обработка ошибок при получении пользователя
                Console.WriteLine($"Ошибка при получении пользователя из Firebase: {ex.Message}");
                return false;
            }

        }
        public async Task<FirebaseToken> VerifyIdTokenAsync(string token)
        {

            if (!FirebaseUtil.CheckAdminInit())
            {
                return null;
            }

            try
            {
                return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            }
            catch (FirebaseAuthException ex)
            {
                // Обработка ошибок при получении пользователя
                Console.WriteLine($"Ошибка при получении пользователя из Firebase: {ex.Message}");
                return null;
            }

        }
        public async Task<bool> RegisterUserAsync(string name, string email, string password)
        {

            if (!FirebaseUtil.CheckAdminInit())
            {
                return false;
            }

            try
            {
                var user = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs()
                {
                    DisplayName = name,
                    Email = email,
                    Password = password
                });

                return user != null;
            }
            catch (FirebaseAuthException ex)
            {
                // Обработка ошибок при получении пользователя
                Console.WriteLine($"Ошибка при получении пользователя из Firebase: {ex.Message}");
                return false;
            }

        }

        internal async Task SendMessageAsync(string receiverId, SendMessageArgs args)
        {
            string? notificationToken = await FireBaseDbService.Instance.GetNotificationTokenAsync(receiverId);

            if (notificationToken == null) return;
            var messageEventType = args.MessageEventTypeEnum;
            
            
            var messageObject = new Message
            {
                Token = notificationToken,
                
                
                Data = new Dictionary<string, string>()
                 {
                     {"MessageType","Message"},

                     {"Message",args.MessageJson},
                     {"MessageEventType",messageEventType},
                     {"SenderId",args.SenderId},
                     {"MessagePublicKey",args.MessagePublicKey}

                 }
            


            };
          
            var res = await FirebaseMessaging.DefaultInstance.SendAsync(messageObject);
          
            Console.WriteLine($"\nSend notification message to receiver id: {args.ReceiverId}");

        }
        internal async Task SendIncomingCallAsync(string receiverId, SignalTargetDataModel dataModel)
        {
            string? notificationToken = await FireBaseDbService.Instance.GetNotificationTokenAsync(receiverId);

            if (notificationToken == null) return;
        
            string dataModelJson= JsonConvert.SerializeObject(dataModel);
          
            Message messageObject = new Message
            {
                Token = notificationToken,
                
              

                Data = new Dictionary<string, string>()
                 {
                     {"MessageType","Incoming call"},

                     {"IncomingCallDataModel",dataModelJson},
                     

                },
                /*
                    Webpush=new WebpushConfig {

                        Headers = new Dictionary<string, string>() {
                            { "TTL","10"}

                        }
                    }, 
                 */


                Android = new AndroidConfig {
                    /*
                        TimeToLive=TimeSpan.FromSeconds(10),
                     */
                   // CollapseKey = "incoming_call", 
                    Priority=Priority.High,
                }
                


            };

            var res = await FirebaseMessaging.DefaultInstance.SendAsync(messageObject);

            Console.WriteLine($"Successfully sent message: {res}");
        }
        internal async Task SendMissedIncomingCallAsync(string? receiverId, SignalTargetDataModel dataModel)
        {
            if (receiverId == null) return;
            string? notificationToken = await FireBaseDbService.Instance.GetNotificationTokenAsync(receiverId);

            string dataModelJson = JsonConvert.SerializeObject(dataModel);
          
            Message messageObject = new Message
            {
                Token = notificationToken,
              

                Data = new Dictionary<string, string>()
                 {
                     {"MissedIncomingCallDataModel",dataModelJson},
                     

                },
          
            };

            var res = await FirebaseMessaging.DefaultInstance.SendAsync(messageObject);

            Console.WriteLine($"\nSuccessfully sent message: {res}");
        }
    }
}
