using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Technical.Test.Business.Interfaces;
using Technical.Test.Business.Models;
using Technical.Test.Business.Models.Validations;

namespace Technical.Test.Business.Services
{
    public class ServerService : BaseService, IServerService
    {
        private readonly IMongoRepository _repository;
        private readonly IMongoSettings _mongoSettings;

        public ServerService(IMongoRepository repository,
            IMongoSettings mongoSettings,
            INotifier notifier) : base(notifier)
        {
            _repository = repository;
            _mongoSettings = mongoSettings;
        }

        public async Task<Server> GetById(Guid id)
        {
            try
            {
                var filterDefinition = Builders<BsonDocument>.Filter.Eq("_id", id);

                var result = await _repository.FindOne(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Servers,
                    filterDefinition);

                return result == null ? null : BsonSerializer.Deserialize<Server>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<List<Server>> GetAll()
        {
            try
            {
                var result = await _repository.FindMany(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Servers);

                return result == null ? null : BsonSerializer.Deserialize<List<Server>>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<Server> Add(Server server)
        {
            try
            {
                if (!ExecuteValidation(new ServerValidation(), server)) return null;

                server.Id = Guid.NewGuid();
                server.CreatedAt = DateTime.UtcNow;
                server.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.Insert(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Servers,
                    server.ToBsonDocument());

                return result == null ? null : BsonSerializer.Deserialize<Server>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<UpdateResult> Update(Server server)
        {
            try
            {
                if (!ExecuteValidation(new ServerValidation(), server)) return null;

                var filterDefinition = Builders<BsonDocument>.Filter.Eq("_id", server.Id);

                var updateDefinition = Builders<BsonDocument>.Update
                    .Set("Name", server.Name)
                    .Set("IP", server.IP)
                    .Set("Port", server.Port)
                    .Set("UpdatedAt", DateTime.UtcNow);

                var options = new UpdateOptions
                {
                    IsUpsert = false
                };

                return await _repository.Update(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Servers,
                    filterDefinition, updateDefinition, options);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<DeleteResult> Delete(Guid id)
        {
            try
            {
                var filterDefinition = Builders<BsonDocument>.Filter.Eq("_id", id);

                return await _repository.Delete(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Servers,
                    filterDefinition);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<string> IsAvailable(Server server)
        {
            try
            {
                var ping = await new Ping().SendPingAsync(server.IP, 3000);

                return Enum.GetName(typeof(IPStatus), ping.Status);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }
    }
}
