using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Technical.Test.DeleteBinary.Models
{
    [BsonIgnoreExtraElements]
    public class Video
    {
        [BsonId]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
