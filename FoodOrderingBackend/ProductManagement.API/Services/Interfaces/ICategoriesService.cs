using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.API.Entities;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace ProductManagement.API.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<ProductCategory> GetAsync(int id);
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<IEnumerable<CategoryWithProductsDto>> GetAllCategoriesWithProductsAsync();
        Task<Result<ProductCategory>> CreateCategoryAsync(ProductCategory category);
        Task<Result> UpdateCategoryAsync(ProductCategory category, ProductCategoryForCreateOrUpdateDto categoryDto);
        Task<Result> DeleteCategoryAsync(ProductCategory category);
    }
}
    