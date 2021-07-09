using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technical.Test.Business.Models;

namespace Technical.Test.Business.Interfaces
{
    public interface IVideoService
    {
        Task UploadBinary(Video video);

        Task<string> DownloadBinary(Guid id);

        Task DeleteBinary(Guid id);

        Task<List<Video>> GetAll(Guid serverId);

        Task<Video> GetById(Guid serverId, Guid id);

        Task<Video> Add(Guid serverId, Video video);

        Task<DeleteResult> Delete(Guid serverId, Guid id);
    }
}
