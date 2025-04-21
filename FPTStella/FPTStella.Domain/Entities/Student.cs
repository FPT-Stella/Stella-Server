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
    public class Student : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("user_id")]
        public Guid UserId { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement("major_id")]
        public Guid MajorId { get; set; }
        [BsonElement("student_code")]
        public string StudentCode { get; set; } = string.Empty;
        [BsonElement("phone")]
        public string Phone { get; set; } = string.Empty;
        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;
    }
}
