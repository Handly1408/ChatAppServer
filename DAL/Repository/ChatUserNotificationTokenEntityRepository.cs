using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ChatUserNotificationTokenEntityRepository : LiteDbRepository<ChatUserNotificationTokenEntity>
    {
       

        private const string TABLE_NAME = "notification_tokens";


		public ChatUserNotificationTokenEntityRepository(string databasePath)
            : base(databasePath, TABLE_NAME) {
        
        }
        public ChatUserNotificationTokenEntityRepository()
           : base(Constants.LITE_DB_DATA_NAME, TABLE_NAME) { }

		public string? GetUserNotificationTokenById(string userNotificationTokenOwnerId)
        {
            return _collection.Find(g => g.TokenUserOwnerId == userNotificationTokenOwnerId).FirstOrDefault()?.NotificationToken;
        }
       
    }

}
