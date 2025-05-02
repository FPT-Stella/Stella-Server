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
    public class Materials : BaseEntity 
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subject_id")]
        public Guid SubjectId { get; set; }
        [BsonElement("material_name")]
        public string MaterialName { get; set; } = string.Empty;
        [BsonElement("material_type")]
        public string MaterialType { get; set; } = string.Empty;
        [BsonElement("material_url")]
        public string MaterialUrl { get; set; } = string.Empty;
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

    }
}
