using DAL.Interfaces;
using LiteDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class EntityBase : IEntity
    {

        
        [JsonIgnore]
        
        public int Id { get ; set; }
       
    }
}
