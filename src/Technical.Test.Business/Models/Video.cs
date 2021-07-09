using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Technical.Test.Business.Models
{
    [BsonIgnoreExtraElements]
    public class Video : InjectionModel
    {
        [Required]
        public string Description { get; set; }

        public int SizeInBytes { get; set; }

        public Guid ServerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        [BsonIgnore]
        public string Base64Content { get; set; }
    }
}
