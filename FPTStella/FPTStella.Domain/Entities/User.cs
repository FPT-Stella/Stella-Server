using FPTStella.Domain.Common;
using FPTStella.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace FPTStella.Domain.Entities
{
    public class User : BaseEntity
    {
        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("password_hash")]
        public string PasswordHash { get; set; }

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public Role Role { get; set; }

        [BsonElement("full_name")]
        public string FullName { get; set; }

        [EmailAddress]
        [BsonElement("email")]
        public string Email { get; set; }
    }
}
