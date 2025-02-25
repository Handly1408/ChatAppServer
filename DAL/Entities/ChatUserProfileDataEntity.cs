namespace DAL.Entities
{
    public class ChatUserProfileDataEntity:ChatContactEntityBase
    {
        public string Email { get; set; } = "";
        public string Status { get; set; } = "";
        public string FriendCode { get; set; } = "";
        public long TimestampLastActivity {  get; set; }
    }
}
