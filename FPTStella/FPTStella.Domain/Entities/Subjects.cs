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
    public class Subjects : BaseEntity
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subjectName")]
        public string SubjectName { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subjectCode")]
        public string SubjectCode { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement("subjectDescription")]
        public string SubjectDescription { get; set; }
        [BsonElement("credits")]
        public int Credits { get; set; }
        [BsonElement("prerequisite")]
        public int Prerequisite { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement("prerequisite_Name")]
        public string PrerequisiteName { get; set; }
        [BsonElement("degree_level")]
        [BsonRepresentation(BsonType.String)]
        public string DegreeLevel { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement("time_allocation")]
        public string TimeAllocation { get; set; }
        [BsonElement("sysllabus_description")]
        [BsonRepresentation(BsonType.String)]
        public string SysllabusDescription { get; set; }
        [BsonElement("studentTask")]
        [BsonRepresentation(BsonType.String)]
        public string StudentTask { get; set; }
        [BsonElement("scoring_scale")]
        public int ScoringScale { get; set; }
        [BsonElement("min-avg_mark_to_pass")]
        public int MinAvgMarkToPass { get; set; }
        [BsonElement("note")]
        [BsonRepresentation(BsonType.String)]
        public string Note { get; set; }
        [BsonElement("topic")]
        [BsonRepresentation(BsonType.String)]
        public string Topic { get; set; }
        [BsonElement("learning_teaching_type")]
        public bool LearningTeachingType { get; set; }
    }
}
