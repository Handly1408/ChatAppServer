using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ChatUserProfileDataEntityRepository : LiteDbRepository<ChatUserProfileDataEntity>
    {
        public ChatUserProfileDataEntityRepository(string databasePath)
            : base(databasePath, "users") { }
        public ChatUserProfileDataEntityRepository()
           : base(Constants.LITE_DB_DATA_NAME, "users") { }
        public ChatUserProfileDataEntity? GetUserProfileDataById(string userProfileDataId)
        {
            return _collection.Find(g => g.ContactId!.Equals(userProfileDataId)).FirstOrDefault();
        }

    }
}
