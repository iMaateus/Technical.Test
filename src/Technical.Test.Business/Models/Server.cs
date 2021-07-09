using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Technical.Test.Business.Models
{
    [BsonIgnoreExtraElements]
    public class Server : InjectionModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string IP { get; set; }

        [Required]
        public int Port { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
