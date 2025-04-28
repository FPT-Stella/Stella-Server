using FPTStella.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Domain.Entities
{
    public class SubjectComboSubjects : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subject_combo_id")]
        public Guid SubjectComboId { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subject_id")]
        public Guid SubjectId { get; set; }
    }
}
