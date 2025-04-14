using FPTStella.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;


namespace FPTStella.Domain.Entities
{
    public class User : BaseEntity
    {
        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("password_hash")]
        public string PasswordHash { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }

        [BsonElement("full_name")]
        public string FullName { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}
