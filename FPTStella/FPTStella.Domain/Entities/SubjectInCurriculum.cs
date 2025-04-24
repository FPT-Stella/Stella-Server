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
    /// SubjectInCurriculum is a class that represents the mapping between subjects and curricula.
    /// </summary>
    public class SubjectInCurriculum : BaseEntity
    {
        /// <summary>
        /// Gets or sets the curriculum ID.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [BsonElement("curriculum_id")]
        public Guid CurriculumId { get; set; }
        /// <summary>
        /// Gets or sets the subject ID.
        /// </summary>
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subject_id")]
        public Guid SubjectId { get; set; }
    }
}
