using FPTStella.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace FPTStella.Domain.Entities
{
    public class ChatHistory : IBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ins_date")]
        public DateTime InsDate { get; set; }

        [BsonElement("upd_date")]
        public DateTime UpdDate { get; set; }

        [BsonElement("del_flg")]
        public bool DelFlg { get; set; }

        [BsonElement("sessionId")]
        public string SessionId { get; set; }

        [BsonElement("messages")]
        public List<Message> Messages { get; set; }
    }

    public class Message
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("data")]
        public MessageData Data { get; set; }
    }

    public class MessageData
    {
        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("tool_calls")]
        public List<object> ToolCalls { get; set; } = new List<object>();

        [BsonElement("invalid_tool_calls")]
        public List<object> InvalidToolCalls { get; set; } = new List<object>();

        [BsonElement("additional_kwargs")]
        public object AdditionalKwargs { get; set; }

        [BsonElement("response_metadata")]
        public object ResponseMetadata { get; set; }
    }

    // Interface for base entity properties
    public interface IBaseEntity
    {
        string Id { get; set; }
        DateTime InsDate { get; set; }
        DateTime UpdDate { get; set; }
        bool DelFlg { get; set; }
    }
}