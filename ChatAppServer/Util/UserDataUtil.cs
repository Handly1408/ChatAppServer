using ChatAppServer.Constants;
using ChatAppServer.Model;
using Newtonsoft.Json;
using System.Text.Json;

namespace ChatAppServer.Util
{
    public class UserDataUtil
    {
        public static async Task SaveStatusAsync(string userId,UserChatStatusModel chatStatusModel)
        {
           
           if (string.IsNullOrEmpty(userId)) { return;}
           if (chatStatusModel == null) {  return; }
            try
            {
                userId = $"{userId}.json";
                if (!Directory.Exists(ServerConstants.USER_DATA_CHAT_STATUS_PATH)) {
                   
                    Directory.CreateDirectory(ServerConstants.USER_DATA_CHAT_STATUS_PATH);
                }

                await File.WriteAllTextAsync(Path.Combine(ServerConstants.USER_DATA_CHAT_STATUS_PATH,userId)
                      , JsonConvert.SerializeObject(chatStatusModel, Formatting.Indented));

            }
            catch (Exception ex) {
                // Обработка ошибок чтения файла или десериализации
                // Вы можете здесь добавить логирование или другие действия
                Console.WriteLine($"An error occurred: {ex.Message}");
              
            }   
   
        }
        public static async Task SaveClientPublicKeyAsync(string? userId, UserPublicKeyModel publicKeyModel)
        {

            if (string.IsNullOrEmpty(userId)) { return; }
            if (publicKeyModel == null) { return; }
            try
            {
                userId = $"{userId}.json";
                if (!Directory.Exists(ServerConstants.USER_PUBLIC_KEYS_PATH))
                {

                    Directory.CreateDirectory(ServerConstants.USER_PUBLIC_KEYS_PATH);
                }

                await File.WriteAllTextAsync(Path.Combine(ServerConstants.USER_PUBLIC_KEYS_PATH, userId)
                      , JsonConvert.SerializeObject(publicKeyModel, Formatting.Indented));

            }
            catch (Exception ex)
            {
                // Обработка ошибок чтения файла или десериализации
                // Вы можете здесь добавить логирование или другие действия
                Console.WriteLine($"An error occurred: {ex.Message}");

            }

        }
        public static async Task<UserPublicKeyModel> GetUserPublicKeyAsync(string userId)
        {

            if (string.IsNullOrEmpty(userId))
            {
                throw new NullReferenceException();
            }

            try
            {
                userId = $"{userId}.json";
                string filePath = Path.Combine(ServerConstants.USER_PUBLIC_KEYS_PATH, userId);

                if (File.Exists(filePath))
                {
                    string text = await File.ReadAllTextAsync(filePath);
                    return JsonConvert.DeserializeObject<UserPublicKeyModel>(text)??new UserPublicKeyModel(userId,"");
                }
                else
                {
                    return new UserPublicKeyModel(userId, ""); // Файл не существует
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок чтения файла или десериализации
                // Вы можете здесь добавить логирование или другие действия
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new UserPublicKeyModel(userId, "");
            }
        }
        /// <summary>
        /// Get user chat status
        /// </summary>
        /// <param name="userId">file name as user id</param>
        /// <returns></returns>
        public static async Task<UserChatStatusModel>  GetStatusAsync(string userId)
        {

            if (string.IsNullOrEmpty(userId))
            {
                throw new NullReferenceException();
            }

            try
            {   userId = $"{userId}.json";
                string filePath = Path.Combine(ServerConstants.USER_DATA_CHAT_STATUS_PATH, userId);

                if (File.Exists(filePath))
                {
                    string text = await File.ReadAllTextAsync(filePath);
                    return JsonConvert.DeserializeObject<UserChatStatusModel>(text)??new UserChatStatusModel(userId,false);
                }
                else
                {
                    return new UserChatStatusModel(userId, false); // Файл не существует
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок чтения файла или десериализации
                // Вы можете здесь добавить логирование или другие действия
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new UserChatStatusModel(userId,false);
            }
        }
    }
}
