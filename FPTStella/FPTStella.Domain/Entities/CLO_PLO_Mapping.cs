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
    /// CLO_PLO_Mapping represents the mapping between Course Learning Outcomes (CLOs) and Program Learning Outcomes (PLOs).
    /// </summary>
    public class CLO_PLO_Mapping : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("clo_id")]
        public Guid CloId { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("plo_id")]
        public Guid PloId { get; set; }
    }
}
