using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Contracts;
using Ordering.Contracts.Models;
using ProductManagement.API.Entities;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Services.Interfaces
{
    public interface IProductsService   
    {
        Task <Product> GetProductAsync(int id);
        Task <IEnumerable<Product>> GetProductsAsync();
        Task<Result<IEnumerable<Product>>> GetProductsByFilterAsync(IEnumerable<ProductFilter> productFilters);
        Task <Result<Product>> CreateProductAsync(Product product);     
        Task <Result> UpdateProductAsync(Product product, ProductForCreateOrUpdateDto productDto);
        Task <Result> DeleteProductAsync(Product product);
        Task DeleteAllProductsInCategoryAsync(int categoryId);
    }
}       
                