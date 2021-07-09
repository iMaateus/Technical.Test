using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technical.Test.Business.Interfaces;
using Technical.Test.Business.Untils;

namespace Technical.Test.Data.Repository
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IEnumerable<IMongoClient> _mongoClients;

        public MongoRepository(IEnumerable<IMongoClient> mongoClients)
        {
            _mongoClients = mongoClients;
        }

        public async Task<BsonDocument> FindOne(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter = null, OptionDefinition option = null)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            return await collection.Find(filter ?? FilterDefinition<BsonDocument>.Empty)
                .Project(option?._projections ?? Builders<BsonDocument>.Projection.Exclude("ignore_all_fields"))
                .FirstOrDefaultAsync();
        }

        public async Task<List<BsonDocument>> FindMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter = null, OptionDefinition option = null)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            return await collection.Find(filter ?? FilterDefinition<BsonDocument>.Empty)
                .Skip(option?._skip)
                .Limit(option?._limit)
                .Sort(option?.Sort)
                .Project(option?._projections ?? Builders<BsonDocument>.Projection.Exclude("ignore_all_fields"))
                .ToListAsync();
        }

        public async Task<List<BsonDocument>> FindManyWithAggregate(int host, string database, string collectionName, List<BsonDocument> pipeline)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            return await collection.Aggregate<BsonDocument>(pipeline).ToListAsync();
        }

        public async Task<BsonDocument> Insert(int host, string database, string collectionName, BsonDocument document)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            await collection.InsertOneAsync(document);

            return document;
        }

        public async Task<List<BsonDocument>> InsertMany(int host, string database, string collectionName, List<BsonDocument> documents)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            await collection.InsertManyAsync(documents);

            return documents;
        }

        public async Task<UpdateResult> Update(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            return await collection.UpdateOneAsync(filter, update, options);
        }

        public async Task<UpdateResult> UpdateMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            return await collection.UpdateManyAsync(filter, update, options);
        }

        public async Task<DeleteResult> Delete(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            return await collection.DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> DeleteMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter)
        {
            var collection = _mongoClients.ElementAt(host).GetDatabase(database).GetCollection<BsonDocument>(collectionName);

            return await collection.DeleteManyAsync(filter);
        }
    }
}
