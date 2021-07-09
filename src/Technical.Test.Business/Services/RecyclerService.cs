using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technical.Test.Business.Interfaces;
using Technical.Test.Business.Models;

namespace Technical.Test.Business.Services
{
    public class RecyclerService : BaseService, IRecyclerService
    {
        private readonly IMongoRepository _repository;
        private readonly IMongoSettings _mongoSettings;
        private readonly IAwsService _awsService;

        public RecyclerService(IMongoRepository repository,
            IMongoSettings mongoSettings,
            IAwsService awsService,
            INotifier notifier) : base(notifier)
        {
            _repository = repository;
            _mongoSettings = mongoSettings;
            _awsService = awsService;
        }

        public async Task DeleteBinary(Recycler recycler)
        {
            try
            {
                var x = await _awsService.SQSSendMessage(new 
                {
                    IsRecycler = true,
                    RecyclerId = recycler.Id.ToString(),
                    Days = recycler.Days,
                    CreatedAt = recycler.CreatedAt
                });
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public async Task<List<Recycler>> GetAll()
        {
            try
            {
                var result = await _repository.FindMany(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Recycler);

                return result == null ? null : BsonSerializer.Deserialize<List<Recycler>>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<Recycler> Add(int days)
        {
            try
            {
                var recycler = new Recycler
                {
                    Days = days,
                    Status = "not running",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _repository.Insert(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Recycler,
                    recycler.ToBsonDocument());

                return result == null ? null : BsonSerializer.Deserialize<Recycler>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }
    }
}
