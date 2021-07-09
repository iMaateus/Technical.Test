using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Technical.Test.Business.Models;

namespace Technical.Test.Business.Interfaces
{
    public interface IServerService
    {
        Task<Server> GetById(Guid id);

        Task<List<Server>> GetAll();

        Task<Server> Add(Server server);

        Task<UpdateResult> Update(Server server);

        Task<DeleteResult> Delete(Guid id);

        Task<string> IsAvailable(Server server);
    }
}
