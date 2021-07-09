using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Technical.Test.Business.Models
{
    public abstract class InjectionModel
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
