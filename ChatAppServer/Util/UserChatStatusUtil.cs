using ChatAppServer.Constants;
using ChatAppServer.Model;
using Newtonsoft.Json;
using System.Text.Json;

namespace ChatAppServer.Util
{
    public class UserChatStatusUtil
    {
        public static async Task SaveStatusAsync(string userId,ContactChatStatusModel chatStatusModel)
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
        /// <summary>
        /// Get user chat status
        /// </summary>
        /// <param name="userId">file name as user id</param>
        /// <returns></returns>
        public static async Task<ContactChatStatusModel?>  GetStatusAsync(string userId)
        {

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            try
            {   userId = $"{userId}.json";
                string filePath = Path.Combine(ServerConstants.USER_DATA_CHAT_STATUS_PATH, userId);

                if (File.Exists(filePath))
                {
                    string text = await File.ReadAllTextAsync(filePath);
                    return JsonConvert.DeserializeObject<ContactChatStatusModel>(text);
                }
                else
                {
                    return new ContactChatStatusModel(userId, false); // Файл не существует
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок чтения файла или десериализации
                // Вы можете здесь добавить логирование или другие действия
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new ContactChatStatusModel(userId,false);
            }
        }
    }
}
