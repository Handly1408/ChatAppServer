namespace ChatAppServer.Constants
{
    public class ServerConstants
    {
        public const string SERVICE_ACCOUNT_PATH = "service_account.json";
        public const string USER_DATA_PATH = "UsersData";
        public static string USER_DATA_CHAT_STATUS_PATH => Path.Combine(USER_DATA_PATH, "UserChatStatus");
        public static string USER_PUBLIC_KEYS_PATH => Path.Combine(USER_DATA_PATH, "UserPublicKeys");


        #region Clients methods Call-Backs
        public const string ON_CLIENT_STATUS_CHANGED = "onClientStatusChanged";
        public const string ON_SEND_MESSAGE_CALL_BACK = "onSendMessageCallBack";
        public const string ON_GET_CONTRACT_PUBLIC_KEY_CALL_BACK = "onGetContactPublicKey";
        public const string ON_RECEIVE_MESSAGE_CALL_BACK = "onReceiveMessage"; 
        #endregion
    }
}
