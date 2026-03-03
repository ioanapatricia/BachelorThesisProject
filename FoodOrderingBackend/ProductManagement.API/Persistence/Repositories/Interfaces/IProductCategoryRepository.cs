using System.Collections.Generic;
using System.Threading.Tasks;
using ProductManagement.API.Entities;

namespace ProductManagement.API.Persistence.Repositories.Interfaces
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        Task<IEnumerable<ProductCategory>> GetAllCategoriesWithImagesAsync();
    }
}
        