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
        [BsonElement("student_code")]
        public string StudentCode { get; set; }
        [BsonElement("phone")]
        public string Phone { get; set; }
        [BsonElement("address")]
        public string Address { get; set; }
    }
}
