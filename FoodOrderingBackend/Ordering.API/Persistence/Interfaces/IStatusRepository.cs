using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.API.Entities;

namespace Ordering.API.Persistence.Interfaces
{
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetAllAsync();
        Task<Status> GetByName(string name);
    }
}
    