using FPTStella.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Domain.Entities
{
    public class Programs : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("major_id")]
        public Guid MajorId { get; set; }
        [BsonElement("program_code")]
        public string ProgramCode { get; set; } = string.Empty;
        [BsonElement("program_name")]
        public string ProgramName { get; set; } = string.Empty;
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

    }
}
