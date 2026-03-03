using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.API.Entities;

namespace Ordering.API.Services.Interfaces
{
    public interface IStatusService
    {
        Task<IEnumerable<Status>> GetAllAsync();
    }
}
