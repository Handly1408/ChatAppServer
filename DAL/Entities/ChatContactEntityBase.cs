using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ChatContactEntityBase:EntityBase
    {
        public string? Name { get; set; } = "";
        public string? ContactId { get; set; } = "";
        public string? ContactOwnerId { get; set; } = "";
        public string? ImgUrl { get; set; } = "";
        [JsonProperty]
        public List<int>? AvatarDefaultColor { get; set; }
        public long timestamp { get; set; }
        public string? ContactType { get; set; } = "";
        public string? Uuid { get; set; } = "";

    }
}
