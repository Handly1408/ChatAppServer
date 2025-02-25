using ChatAppServer.Services;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;

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
                    ""private_key_id"": ""e38e2696fe215490769d14a7ba927026b869f2cb"",
                    ""private_key"": ""-----BEGIN PRIVATE KEY-----
MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDKcdQtARB+bwG7
n2/Ef2UJ48OffHJdNTpCLoivrtcskocOjfLvetyN00n7Gg1CK2gTIzQ1xLIJOFan
VK/6xgSlzmvJVqwZOrBAk9PuAxVgLgT/gbGFt2IL8mnxoeZofw2fANBdBfOm3Zrz
mpQnwY26m8R1ClI6gGlke+unWiOX3enzpva9/oe0C2GpwrHJJ28ZIh7Anwoc8Wt4
MiISOlB59OnDu3LFnX9+Woa9Qs+5uKahcIvxkzxzGNLDg5j2yOyJOzgSOPpZ/u4S
QUTV09QtaY3JiOOO2IEa59VzswvIuhI4fNhF1HcHBEyxEQlZlUvqg0u0hZ9UwnVq
VhHXjAEtAgMBAAECggEACLCSlkzW872AhqbDWhXnmJowrXSJY0n1ZDpOO0Ghy0yq
Z+FOn5Ms9cNLzzbPSKlj4EH56/eXBMwTZs2SP1RR+BXOqUtYrZLIAR2ebv6RjlKr
X100sGznYoDJF2bOHcBi2C5lbeqN7jMKvFfvY6glP7fIaQd9f6axNMT8Eah4ocNy
WdB0UDJjZJM+15veEnO5NHRsvXo+lpcPqJtun9MbuhzRQTHqAlTtT9aiUkd4FaZo
QNqWilIyC8Eq1TOvSLk/qr5PeAhueUGz4sNUccvU3MhKMi71tgcuUofVeZ/YqyYM
br9T8obKe6IlN8Z4gk9u2cMLLLRNRvy44NHdZ5ljKQKBgQDmVdTQzWG3DRaeqarH
CmvFCcBS2L3B4VYLnNWYqW8NoM3UlCn/ED33DSN5zUFmn6e/TY343mb9HsF63kUl
WqfXMnYhHqn4/KZtgioTMWQDa+1SuicgI/NB3CD1nGo1ED1tScLfKpB0KUkh+SPH
yaghMR/O8P8/xB80PttZTujv6wKBgQDhAG9Jp5ruZKDE2AILdOXrr9z3O1nS90kv
zB2IKZoZlXU9nCjAEUgGrxoELcjvisNg2lSQkL/7UyiyVqqzRkFdIWdwXY4hTfXa
ShHjA81tZ/m+yd9Gg0N+YwB34nLVwrHeRcRN6e+R9tnt2uuXmYRtvBusBu9AD9vz
hAH4ci2lRwKBgQDDEBs7fUNsbzQhyBQtngCojFqIjq8c20UwbnBhadP3mZ/WkGeE
b2/aohDSHOZvD3RTSU2bXlWftQUrlcK/IKgVUdHCuKI/j08uXFZfjKtjTmpcbfOm
f2uJ8e8jsYQsTgWHYTkcH4LYLQKXN9N6Zafx/BUw/t1bVi8K4tSGLJKOYwKBgQDf
JPM59QNuklXvYtsESbcM+6kFeNMoqx7mTGStebTe0p8nrwurEHiCSH5gzpeNGe2S
zHU5upTryBmAZPt+OJrypgJUKjSfSxYcm4EJx+egLZI+aS8KW3xAiCcavnZtEV4d
tsttCikSdeLERm2IODRiMECxZ184BYJ6C914bKRgWQKBgEsrGc1x76sX34hy2SLJ
vrPAz2sUJ6ucOsvTVea9JqSIEO79sgEySw8yGZsc3jEDhhIC36/hJYN+JCDafVvb
y5JaYfNCtdRHAaUYvAiCz9GvaMI1Wc91sK7M7+hLDlSCWfxYZ9tQdinRJS1GkRK7
75WFOWYyIl5O4ul0nf24jF+h
-----END PRIVATE KEY-----
"",
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
            {
 
}
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
            if (!_projectId.IsNullOrEmpty()) return _projectId;
            var config = Program.App.Configuration.GetSection("project_id");
            return _projectId = config?.Value ?? null;

        }

    }
}
