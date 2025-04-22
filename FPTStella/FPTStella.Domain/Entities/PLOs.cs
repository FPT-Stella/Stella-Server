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
    /// PLOs is a class that represents a Program Learning Outcome in the system.
    /// </summary>
    public class PLOs : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("curriculum_id")]
        public Guid CurriculumId { get; set; }

        [BsonElement("plo_name")]
        public string PloName { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
    }
}
