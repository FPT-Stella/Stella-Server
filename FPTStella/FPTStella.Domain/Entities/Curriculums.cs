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
    public class Curriculums : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("program_id")]
        public Guid ProgramId { get; set; }
        [BsonElement("curriculum_code")]
        public string CurriculumCode { get; set; } = string.Empty;
        [BsonElement("curriculum_name")]
        public string CurriculumName { get; set; } = string.Empty ;
        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
        [BsonElement("total_credit")]
        public int? TotalCredit { get; set; }
        [BsonElement("start_year")]
        public int? StartYear { get; set; }
        [BsonElement("end_year")]
        public int? EndYear { get; set; }
    }
}
