

namespace FPTStella.Contracts.DTOs.Students
{
    public class UpdateStudentDto
    {
        public string StudentCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Guid MajorId { get; set; } ;
    }
}
