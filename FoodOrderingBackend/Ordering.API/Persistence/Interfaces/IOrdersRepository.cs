using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Contracts;
using Ordering.API.Entities;

namespace Ordering.API.Persistence.Interfaces
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetAsync(string id);
        Task<Result<Order>> CreateAsync(Order order);
    }
}
