using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Technical.Test.DeleteBinary.Models
{
    [BsonIgnoreExtraElements]
    public class Recycler
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }
    }
}
