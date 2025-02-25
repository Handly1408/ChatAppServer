namespace DAL.Entities
{
    public class ChatUserNotificationTokenEntity : EntityBase
    {

        public string? TokenUserOwnerId { get; set; } = "";
        public string? NotificationToken { get; set; } = "";

      
    }
}
