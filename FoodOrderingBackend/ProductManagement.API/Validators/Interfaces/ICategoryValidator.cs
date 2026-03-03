using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace ProductManagement.API.Validators.Interfaces
{
    public interface ICategoryValidator
    {
        Result ValidateCategory(ProductCategoryForCreateOrUpdateDto productCategoryForCreateOrUpdateDto);
    }
}
