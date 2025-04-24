using FPTStella.Domain.Common;
using FPTStella.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace FPTStella.Domain.Entities
{
    public class Account : BaseEntity
    {
        [BsonElement("username")]
        public string Username { get; set; } = string.Empty; 

        [BsonElement("password_hash")]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public Role Role { get; set; }

        [BsonElement("full_name")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;
    }
}
