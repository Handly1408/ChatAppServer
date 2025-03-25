using ChatAppServer.Services;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace ChatAppServer.Util
{
    public class FirebaseUtil
    {

        private static string? _projectId;
        private static string? _filePath;
        private static string _serviceAccountFile = @"
{
                ""type"": ""service_account"",
                ""project_id"": ""chat-ab4ff"",
                ""private_key_id"": ""b779611e74c2226beedeaec660e09a1e581b14b2"",
                ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCW4V7je6pvuFiU\nUI76NFQFdtECm3kvdkfmnfYV0hp39pOIoPmOK1iKGP9OtXYLtMwTSJNf+B3PVp/D\nQHckk7sTEMtgkrLk+22Xp7DKSdy5bGsUpLbFRc8RM9rboxA6VIFF7uYQo0Om57hV\nj8WcCWYz6HIpX+YQ5Lz7bg7bTSYuop2JlyRPG1mb3lPKwdPu+RfdxjfeyazQlgit\nWxe4iXzNQPQ0gVIa4nGVsXQIlwGVHSP6cMYGsLMJ7AQimMqYjVaMqKrkgz5LUdXN\nHWsXh7O3m9nnGKX9xVtElNUOZkdIyVS+53lwJzTC5kkYG4aaxMP1np0lmSSj5TGX\nFKs7d4elAgMBAAECggEACRhYUTkG7uRLWKjGLvshC9GcAKl7w7qVqSdL/92SBrm9\n2SkE/3GJoozRCmmsuOttjYWyNpaHwNQis3vVwsYcqztNBeBfTtIris5gZ+1NvtK0\nM21zbrm0z/9oJGsfDbkYMC2aFOuBPD+F0IiRu68jB/gti49yQwHwNMLaET+vEHVZ\nalasjIlD/MjiC2gpXMYurCC7nVNUT8nAImgxZue61mpzf9TgrZj62dXLp/TygvOn\nGTm3BdbdKHYUh+CKmPQHZiapK58R+hskLGEnlMi/fApduNqRzvw1GhgZNboX805v\nzRoZrUXfbVjht2YmHIrzY6TbvJYaklYUf3QxtUUIaQKBgQDUoJXEivq0O/tdTbdk\nI/T13zdT5V21Dr9e4j4AWd5/5KpofWiQFRpGEg3JRjyTUlH6fu3pTvmQ1MkeMFHP\nRB5wvlzhBbhiWYVcjUGKTh8E6t7iQ0hXjKNNdQ1dRw5koqySmBCFCl40xtiMJzzM\nMRQufcB43fmX4MgqKCFkRk9TrQKBgQC1qFsJ0PAJ5KZTc/t6MTmu4K+T1tfuzp3M\n5lMp3S62M6Kp31MAjoW/UfBIkIlZLiY9ygSyE0qSPiveQ9A0ODs6ky2IagtFGrqT\ncvngvJmruuPPHrHHvVfEmVDgFMKjX0mItXudziYNJt9JwuW52B9BvsPeD6gdaWSB\nBifFdiNC2QKBgGQ3P/+/5YQXlVUXsS1QyT8tsx0myTDyP36QMdrmgLlU5ICVQzhN\nyfFmxvqr0Rc6wIrJUocZgwUhVZ2V9qkzGQnYn1wr3wjz2qiRp2dxhKtDrEOGpM0m\n6z0xw1fIit8h0SbS505dYaOMaTo9sCkppLJ48MPHE8Hi31UHTx29CdyNAoGASCoo\neF7TbOqzlIWHyoQRqUJDHdUElS7pi0OYmBHxA5rBrQA15gu4YEe5z+nzVkU6FePS\nN7foMO6X2MVRydzo0p0zQnT2Iy66HedYEwp5dbpZ06ca8wFqA4TwlwRISkSJXtDR\n2d/SR/lATPoq7shHqoORJf9MRtrefU8nr4799QkCgYEAnvXgi7WI5ZxyIMToqKRZ\nxi7Uyy+UqtkN0l3bulcS2xBjY941OE42OoDgAfRq9sLWmMgTdmhmXP/WYxGNxHVa\nS8P1pTGKSzX7dPBimpxzWXGef8JvQmH1Rvzj2ofN2THrpDig3724z0uitdrcbgqW\noh9BNgwN+Y6e1mu1Y9L7XcM=\n-----END PRIVATE KEY-----\n"",
                ""client_email"": ""firebase-adminsdk-67od2@chat-ab4ff.iam.gserviceaccount.com"",
                ""client_id"": ""116774589949156158202"",
                ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                ""token_uri"": ""https://oauth2.googleapis.com/token"",
                ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-67od2%40chat-ab4ff.iam.gserviceaccount.com"",
                ""universe_domain"": ""googleapis.com""
}";
        public static bool CheckAdminInit()
        {

            if (!FireBaseAdminService.Instance.Initialized)
            {
                Console.WriteLine("Firebase не инициализирован. Пожалуйста, проверьте конфигурацию.");
                return false;
            }
            return true;
        }
     
        public static string GetServiceAccountPath()
        {
            if (!_filePath.IsNullOrEmpty()) return _filePath;

            _filePath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())) + ".json";

            File.WriteAllText(_filePath, _serviceAccountFile);
            File.SetAttributes(_filePath, FileAttributes.Hidden);
            return _filePath;
        }
        public static void DeletePathToServiceAccount()
        {
            File.Delete(_filePath);
            _filePath = null;
        }

        public static string? GetProjectId()
        {
            if (!_projectId.IsNullOrEmpty()){
                Console.ForegroundColor = ConsoleColor.Yellow;
        
              
                Console.WriteLine($"\n Project id:{_projectId}");
                Console.ResetColor();
                return _projectId; 
            }
             
            JObject serviceAccount=JObject.Parse(_serviceAccountFile);
            _projectId = (string)serviceAccount["project_id"]!;
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write($"\n Parse project id:{_projectId}");
            Console.ResetColor();

            return _projectId ?? null; 

        }

    }
}
