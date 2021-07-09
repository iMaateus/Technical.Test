using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Technical.Test.DeleteBinary.Models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Technical.Test.DeleteBinary
{
    public class Function
    {
        //Melhor solução usar AWS KSM para armazenar esses acessos.
        private const string ACCESS_KEY = "SUA-AWS-ACCESS-KEY";
        private const string SECRET_KEY = "SUA-AWS-SECRET-KEY";
        private const string S3_BUCKET = "bucket-technical-test-seventh";
        private const string MONGO_URL = "SEU-MONGODB-URL";
        private const string MONGO_DATABASE = "technicaltest";

        private IMongoClient CreateMongoConnection()
        {
            MongoClientSettings mongo = MongoClientSettings.FromConnectionString(MONGO_URL);
            mongo.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            return new MongoClient(mongo);
        }

        private async Task DeleteBinary(List<string> keys)
        {
            using (AmazonS3Client client = new AmazonS3Client(ACCESS_KEY, SECRET_KEY, RegionEndpoint.SAEast1))
            {
                foreach (var key in keys)
                {
                    try
                    {
                        DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                        {
                            BucketName = S3_BUCKET,
                            Key = key
                        };

                        await client.DeleteObjectAsync(deleteRequest);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ID: {0} Exception, Reason: {1}", key, ex.Message);
                    }
                }
            }
        }

        private async Task<List<Video>> GetVideosByDays(IMongoClient mongoClient, DateTime createdEvent, int days)
        {
            var filterDefinition = Builders<BsonDocument>.Filter.Lt("CreatedAt", createdEvent.AddDays(days * -1));

            var collection = mongoClient.GetDatabase(MONGO_DATABASE).GetCollection<BsonDocument>("videos");

            var result = await collection.Find(filterDefinition).ToListAsync();

            return BsonSerializer.Deserialize<List<Video>>(result.ToJson());
        }

        private async Task UpdateRecycler(IMongoClient mongoClient, string id, bool isFinished)
        {
            var filterDefinition = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

            var updateDefinition = Builders<BsonDocument>.Update
                .Set("Status", isFinished ? "finished" : "running")
                .Set(isFinished ? "FinishedAt" : "StartedAt", DateTime.UtcNow);

            var options = new UpdateOptions
            {
                IsUpsert = false
            };

            var collection = mongoClient.GetDatabase(MONGO_DATABASE).GetCollection<BsonDocument>("recycler");

            await collection.UpdateOneAsync(filterDefinition, updateDefinition, options);
        }

        private async Task DeleteVideos(IMongoClient mongoClient, DateTime createdEvent, int days)
        {
            var filterDefinition = Builders<BsonDocument>.Filter.Lt("CreatedAt", createdEvent.AddDays(days * -1));

            var collection = mongoClient.GetDatabase(MONGO_DATABASE).GetCollection<BsonDocument>("videos");

            await collection.DeleteManyAsync(filterDefinition);
        }

        public async Task<string> FunctionHandler(JObject sqsEvent, ILambdaContext context)
        {
            try
            {
                Body body = JsonSerializer.Deserialize<Body>(sqsEvent["Records"].FirstOrDefault()["body"].ToString());

                if (body.IsRecycler)
                {
                    var mongoClient = CreateMongoConnection();

                    var videos = await GetVideosByDays(mongoClient, body.CreatedAt, body.Days);

                    await UpdateRecycler(mongoClient, body.RecyclerId, false);

                    if (videos.Count > 0)
                    {
                        var keys = videos.Select(x => x.Id.ToString()).ToList();
                        await DeleteBinary(keys);

                        await DeleteVideos(mongoClient, body.CreatedAt, body.Days);
                    }

                    await UpdateRecycler(mongoClient, body.RecyclerId, true);
                }
                else
                {
                    var keys = new List<string> { body.Key };

                    await DeleteBinary(keys);
                }

                return string.Format("Success");
            }
            catch (Exception ex)
            {
                return string.Format("Error, Reason: {0}", ex.Message);
            }
        }
    }
}
