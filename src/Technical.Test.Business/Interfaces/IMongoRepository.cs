using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technical.Test.Business.Untils;

namespace Technical.Test.Business.Interfaces
{
    public interface IMongoRepository
    {
        Task<BsonDocument> FindOne(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter = null, OptionDefinition option = null);

        Task<List<BsonDocument>> FindMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter = null, OptionDefinition option = null);

        Task<List<BsonDocument>> FindManyWithAggregate(int host, string database, string collectionName, List<BsonDocument> pipeline);

        Task<BsonDocument> Insert(int host, string database, string collectionName, BsonDocument document);

        Task<List<BsonDocument>> InsertMany(int host, string database, string collectionName, List<BsonDocument> documents);

        Task<UpdateResult> Update(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options);

        Task<UpdateResult> UpdateMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter, UpdateDefinition<BsonDocument> update, UpdateOptions options);

        Task<DeleteResult> Delete(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter);

        Task<DeleteResult> DeleteMany(int host, string database, string collectionName, FilterDefinition<BsonDocument> filter);
    }
}
