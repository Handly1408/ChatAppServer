using ChatAppServer.Util;
using Google.Cloud.Firestore;

namespace ChatAppServer.Services
{
    public class FireBaseDbService : FirebaseServiceBase<FireBaseDbService>

    {
        private FirestoreDb _firestoreDb;

        public FireBaseDbService()
        {
            try
            {
                Initialized = true;
            }
            catch (Exception ex)
            {
                // Обработка ошибок при инициализации Firebase
                Console.WriteLine($"Ошибка при инициализации Firebase: {ex.Message}");
                Initialized = false;
            }
        }
        internal async Task<string?> GetNotificationTokenAsync(string clientId)
        {

            if (_firestoreDb==null) {
                SetEnvironmentVeriable();
                _firestoreDb = await FirestoreDb.CreateAsync(FirebaseUtil.GetProjectId());
                FirebaseUtil.DeletePathToServiceAccount();
            }
            // Проверка наличия корректного идентификатора проекта Firebase
            if (_firestoreDb != null)
            {
                try
                {
                    var snapshot = await _firestoreDb.Collection("users").Document(clientId).GetSnapshotAsync();

                    // Проверка наличия документа
                    if (snapshot.Exists)
                    {
                        // Получение значения поля "notificationToken" из документа
                        return snapshot.GetValue<string>("notificationToken");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при инициализации Firebase: {ex.Message}");

                }
            }

            // Возврат значения по умолчанию (например, null) в случае ошибки или отсутствия данных
            return null;

        }
        private  void SetEnvironmentVeriable()
        {
           
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", FirebaseUtil.GetServiceAccountPath());

        }
    

    }

}
