using FPTStella.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Domain.Entities
{
    public class Tools : BaseEntity
    {
        [BsonElement("tool_name")]
        public string ToolName { get; set; } = string.Empty;
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
    }
}
