using ChatAppServer.Util;

namespace ChatAppServer.Model
{
    public class UserPublicKeyModel
    {
        public string UserId { get; set; } 
        public string PublicKey { get; set; }

        public UserPublicKeyModel(string userId, string publicKey)
        {
            UserId = userId;
            PublicKey = publicKey;
        }

       



        
        
     
    }

}
