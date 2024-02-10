using ChatAppServer.Util;

namespace ChatAppServer.Model
{
    public class ContactChatStatusModel
    {
        public string ContactId { get; set; }
      
        public long Timestamp { get; set; }
        public bool IsActive { get; set; }

        // Конструктор
        public ContactChatStatusModel(string contactId, bool isActive)
        {
            
            ContactId = contactId;
            IsActive = isActive;
            Timestamp=TimeUtil.GetCurrentTimeMilliseconds();
        }
        public ContactChatStatusModel(string contactId, bool isActive,long timestamp)
        {

            ContactId = contactId;
            IsActive = isActive;
            Timestamp = timestamp;
        }
        public ContactChatStatusModel()
        {
            
        }
    }

}
