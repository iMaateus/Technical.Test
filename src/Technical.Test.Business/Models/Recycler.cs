using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Technical.Test.Business.Models
{
    [BsonIgnoreExtraElements]
    public class Recycler
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Status { get; set; }

        public int Days { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? FinishedAt { get; set; }
    }
}
