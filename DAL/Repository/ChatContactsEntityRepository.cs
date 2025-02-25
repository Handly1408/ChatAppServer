using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class ChatContactsEntityRepository : LiteDbRepository<ChatContactEntity>
    {
        public ChatContactsEntityRepository(string databasePath)
            : base(databasePath, "chat contacts") { }
        public ChatContactsEntityRepository()
           : base(Constants.LITE_DB_DATA_NAME, "chat contacts") { }
        public ChatContactEntity? GetByContactId(string contactId)
        {
            return _collection.Find(g => g.ContactId == contactId).FirstOrDefault();
        }
        public ChatContactEntity? GetByContactId(string contactId,string ownerId)
        {
            return _collection.Find(g => g.ContactId!.Equals(contactId) && g.ContactOwnerId!.Equals(ownerId)).FirstOrDefault();
        }
    }

}
