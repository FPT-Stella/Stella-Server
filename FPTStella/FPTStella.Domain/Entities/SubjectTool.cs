using FPTStella.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Domain.Entities
{
    public class SubjectTool : BaseEntity
    {
        public Guid SubjectId { get; set; }    
        public Guid ToolId { get; set; }        
    }
}
