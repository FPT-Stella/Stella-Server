

namespace FPTStella.Contracts.DTOs.Students
{
    public class StudentDto
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string MajorId { get; set; }
        public required string StudentCode { get; set; }
        public required string Phone { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
