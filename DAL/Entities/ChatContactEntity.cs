using DAL.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ChatContactEntity : ChatContactEntityBase
    {
       
        public string? ContactPrivacyType { get; set ; }
        public List<string>? Members { get; set; }

      
    }
}
