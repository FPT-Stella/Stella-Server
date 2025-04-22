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
    /// <summary>
    /// POs is a class that represents a Program Outcome in the system.
    /// </summary>>
    public class POs : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("program_id")]
        public Guid ProgramId { get; set; }
        [BsonElement("po_name")]
        public string PoName { get; set; } = string.Empty;
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
    }
}
