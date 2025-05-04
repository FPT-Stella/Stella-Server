using FPTStella.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

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

        [BsonElement("clo_name")]
        public string CloName { get; set; } = string.Empty;

        [BsonElement("clo_details")]
        public string CloDetails { get; set; } = string.Empty;

        [BsonElement("lo_details")]
        public string LoDetails { get; set; } = string.Empty;
    }
}