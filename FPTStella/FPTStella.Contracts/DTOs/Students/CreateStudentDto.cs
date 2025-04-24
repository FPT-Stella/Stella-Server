

namespace FPTStella.Contracts.DTOs.Students
{
    public class CreateStudentDto
    {
        public required string MajorId { get; set; }
        public required string StudentCode { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
