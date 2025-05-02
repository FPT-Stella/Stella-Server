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
    /// <summary>
    /// CLOs (Course Learning Outcomes) entity representing learning outcomes for subjects.
    /// </summary>
    public class CLOs : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subject_id")]
        public Guid SubjectId { get; set; }

        [BsonElement("clo_details")]
        public string CloDetails { get; set; } = string.Empty;
    }
}
