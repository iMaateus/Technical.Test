using System.Collections.Generic;
using System.Threading.Tasks;
using Technical.Test.Business.Models;

namespace Technical.Test.Business.Interfaces
{
    public interface IRecyclerService
    {
        Task<List<Recycler>> GetAll();

        Task<Recycler> Add(int days);

        Task DeleteBinary(Recycler recycler);
    }
}
