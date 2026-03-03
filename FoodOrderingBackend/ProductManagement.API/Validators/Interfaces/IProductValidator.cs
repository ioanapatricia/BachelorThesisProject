using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Validators.Interfaces
{
    public interface IProductValidator
    {
        Task<Result> ValidateProduct(ProductForCreateOrUpdateDto productForCreateOrUpdate);
    }
}
