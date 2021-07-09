using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Technical.Test.Business.Interfaces;
using Technical.Test.Business.Models;
using Technical.Test.Business.Models.Validations;

namespace Technical.Test.Business.Services
{
    public class VideoService : BaseService, IVideoService
    {
        private readonly IMongoRepository _repository;
        private readonly IMongoSettings _mongoSettings;
        private readonly IAwsService _awsService;

        public VideoService(IMongoRepository repository,
            IMongoSettings mongoSettings,
            IAwsService awsService,
            INotifier notifier) : base(notifier)
        {
            _repository = repository;
            _awsService = awsService;
            _mongoSettings = mongoSettings;
        }

        public async Task UploadBinary(Video video)
        {
            try
            {
                if (!ExecuteValidation(new VideoValidation(), video)) return;

                byte[] bytes = Convert.FromBase64String(video.Base64Content);
                video.Base64Content = null;
                video.SizeInBytes = bytes.Length;
                video.Id = Guid.NewGuid();

                await _awsService.S3UploadBinary(video.Id.ToString(), bytes);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public async Task<string> DownloadBinary(Guid id)
        {
            try
            {
                return await _awsService.S3DownloadBinary(id.ToString());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task DeleteBinary(Guid id)
        {
            try
            {
                await _awsService.SQSSendMessage(new { Key = id.ToString() });
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
            }
        }

        public async Task<List<Video>> GetAll(Guid serverId)
        {
            try
            {
                var filterDefinition = Builders<BsonDocument>.Filter.Eq("ServerId", serverId);

                var result = await _repository.FindMany(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Videos,
                    filterDefinition);

                return result == null ? null : BsonSerializer.Deserialize<List<Video>>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<Video> GetById(Guid serverId, Guid id)
        {
            try
            {
                var filterDefinition = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("_id", id),
                    Builders<BsonDocument>.Filter.Eq("ServerId", serverId));

                var result = await _repository.FindOne(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Videos,
                    filterDefinition);

                return result == null ? null : BsonSerializer.Deserialize<Video>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<Video> Add(Guid serverId, Video video)
        {
            try
            {
                video.ServerId = serverId;
                video.CreatedAt = DateTime.UtcNow;
                video.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.Insert(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Videos,
                    video.ToBsonDocument());

                return result == null ? null : BsonSerializer.Deserialize<Video>(result.ToJson());
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }

        public async Task<DeleteResult> Delete(Guid serverId, Guid id)
        {
            try
            {
                var filterDefinition = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("_id", id),
                    Builders<BsonDocument>.Filter.Eq("ServerId", serverId));

                return await _repository.Delete(
                    _mongoSettings.TechnicalTest.ClusterId,
                    _mongoSettings.TechnicalTest.Database,
                    _mongoSettings.Collections.Videos,
                    filterDefinition);
            }
            catch (Exception ex)
            {
                Notify(ex.Message);
                return null;
            }
        }
    }
}
