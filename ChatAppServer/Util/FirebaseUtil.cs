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
                    ""project_id"": ""chatix-be382"",
                    ""private_key_id"": ""4f35f6bf3384f26c7cc8c45ef5ce1cf40d8486a3"",
                    ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDHHBDfZKzgwsRG\n9l3BDfmqeHJEW/rHN4i00uBy8IJwj5GKDd59T6RJWtTJt35GcyyvAJISntmPrbiJ\nMtnGnYbIDEs2JPGvThkbUZggHQoeBxM2YUiJKQl21CAPdnNd+1/HJLGWwmRcFljk\n1iNi5IbevV2hVeBc8VQU/k2q63hR47twLaWDLMqDONMITxlFGnOA+eOYhWEQLBLn\nDAWQogO0ZCyhzYxwXJWT3RibVHO7THkFbPjLlNF3wAyuVUB+CAQnA5IcwKUbjCPU\nWZeXLwN8P0JzVJl5BQ9fvY4JMm96Si/lvn3dRMTauaggObf6w8PNgCQP+aX8rA2X\nLsnj4JLTAgMBAAECggEAF2RdORmXU+qd66ZbGmi9acEvz4cs3cXQ+kmCFA7cjEPZ\nuenqNzepmfKhaDd2DunOGJtNDgH6lm427Wt7eWGdaZhEJ87MVCetPG/oGvG+dAup\nvqxMPM0E7yB5ycdFQ4faV0eg05DwAH+hnvTngGFP1RBTVTaFHvY9RiAgWNHEN36A\nKVuh/dzOUo8/hZZTIjiD7kmjMSTzps/HaydWBA3xGYwizIrt4EsdweKNQjO4Unci\nd3w5k4UzWbci6hN/mX4N2bZf/7k7zAF27YAVZJMVgV9keJ5o3gRPjfiPxnlOrK01\nKTWp9z6QcNxJlGVtYD0HT0wB4DHk/RLAoTOcLiwx0QKBgQDwmEYJv2cDuIQobWQP\nLGgQfExHjgtn1pxKa6Ee0BXm3X0hpCmKZ5GVicleDoFx1MtThAn6DeLTkB629rfY\nYGsLp5A/fRhc98ddVCc5Ap/lLJ3A5eoIGu5a1TA8QVkb52SGCFTjnNORopiFXOTi\nvZZBwhrO0lNd7pL6lO+zRM4HsQKBgQDT28j5flsGVcAVsFGjY5oG5F/kELj0KYm5\nY6tdiZraeZklVajZLj+E72rz1Fwoi82Jw3ZpEBfv8WwmHUa80U1C+7G1Egaf+bM4\n4NFWB7NoLMoXcClEdxpCsAUKZOF78EOUmt7h2zcCITad2CleGkUDa/1w5a44y34L\nYTKGMgDnwwKBgQDAGlWR4un15pZfUTjbhLFyuEyPtbMVrh4MrfNwDuXt+Fu5SMKC\naD+uDt5h42Mn6KiQfpUYu42pLyHXJReWBSZzn89lYhTZ5l2g/1hyBM52xjEPnIRJ\nBHfcvuxMsEDeLtvsySo+szGazyDJG2sGQCqBRuLw0K8QCTYCLnlVwlJfgQKBgEkE\n65PyVPUuAPJ4vuqNnbzaYaJ9kQG2f12CWMH4d4Ltfjc7+uI/6myrCDXZ75mZ9dGR\nqPI1NcrhbuEEHileCj8X+PvrppTkvzzPX9dC+DbjmRPS+KY8VrS9bj9S3dSVzvUd\nG8zEfYUiZWyp4Dgq3bZ8NfH/d7jjiiHp8jwGaAF5AoGAEfdbFwlX4dsdTvV6Jgak\n0n2boumEGuCwW9KCHfAV1cMTlYgLUJTvdtdyys18KkevpjbmL0glM25xHzRVn/kY\noLSvNI2A7arrarJwyYhR44ONNSUUrLF0mnAlhHFxOhQh2vkecIR9iJJbuMvqvrJJ\nNoTXkLiUTWHgcGLsTWUDYUg=\n-----END PRIVATE KEY-----\n"",
                    ""client_email"": ""firebase-adminsdk-todbl@chatix-be382.iam.gserviceaccount.com"",
                    ""client_id"": ""101347413859267099682"",
                    ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
                    ""token_uri"": ""https://oauth2.googleapis.com/token"",
                    ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
                    ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-j586k%40chatix-be382.iam.gserviceaccount.com"",
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
            if (!_projectId.IsNullOrEmpty()) return _projectId;
            var config = Program.App.Configuration.GetSection("project_id");
            return _projectId = config?.Value ?? null;

        }

    }
}
