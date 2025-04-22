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
    /// PO_PLO_Mapping is a class that represents the mapping between Program Outcomes (POs) and Program Learning Outcomes (PLOs).
    /// </summary>
    public class PO_PLO_Mapping : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("po_id")]
        public Guid PoId { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("plo_id")]
        public Guid PloId { get; set; }
    }
}
