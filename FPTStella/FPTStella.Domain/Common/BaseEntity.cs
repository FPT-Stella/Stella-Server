using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace FPTStella.Domain.Common
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; set; }

        [BsonElement("ins_date")]
        public DateTime InsDate { get; set; }

        [BsonElement("upd_date")]
        public DateTime UpdDate { get; set; }

        [BsonElement("del_flg")]
        public bool DelFlg { get; set; }
    }
}
