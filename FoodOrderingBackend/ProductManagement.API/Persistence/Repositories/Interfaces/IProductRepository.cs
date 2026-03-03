using System.Collections.Generic;
using System.Threading.Tasks;
using Ordering.Contracts.Models;
using ProductManagement.API.Entities;

namespace ProductManagement.API.Persistence.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllByFilterAsync(IEnumerable<ProductFilter> productFilters);
        Task<IEnumerable<Product>> GetAllProductsByCategoryAsync(int id);
    }   
}
    