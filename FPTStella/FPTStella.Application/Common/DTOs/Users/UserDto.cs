﻿
namespace FPTStella.Application.Common.DTOs.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
