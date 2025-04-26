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
    public class SubjectCombo : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("program_id")]
        public Guid ProgramId { get; set; }
        [BsonElement("combo_name")]
        public string ComboName { get; set; } = string.Empty;
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
        [BsonElement("program_outcome")]
        public string ProgramOutcome { get; set; } = string.Empty;
    }
}
