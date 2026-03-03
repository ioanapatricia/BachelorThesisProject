using System.Threading.Tasks;
using Global.Contracts;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace ProductManagement.API.Validators
{
    public class CategoryValidator : ICategoryValidator
    {
        private readonly IImageValidator _imageValidator;

        public CategoryValidator(IImageValidator imageValidator)
        {
            _imageValidator = imageValidator;
        }
        public Result ValidateCategory(ProductCategoryForCreateOrUpdateDto category)
        {
            var categoryPrimitivesValidationResult = ValidatePrimitives(category);
            if (categoryPrimitivesValidationResult.IsFailure)
                return categoryPrimitivesValidationResult;

            var categoryBannerValidationResult = _imageValidator.ValidateImage(category.Banner);
            if (categoryBannerValidationResult.IsFailure)
                return categoryBannerValidationResult;

            var categoryLogoValidationResult = _imageValidator.ValidateImage(category.Logo);
            if (categoryLogoValidationResult.IsFailure)
                return categoryLogoValidationResult;

            return Result.Ok();

        }

        private Result ValidatePrimitives(ProductCategoryForCreateOrUpdateDto category)
        {
            if (string.IsNullOrEmpty(category.Name))
                return Result.Fail("Category name is required.");

            if (category.Name.Length < 3 || category.Name.Length > 30)
                return Result.Fail("Category name should be between 3 and 30 characters.");

            if(category.SortingOrderOnWebpage == default)
                return Result.Fail("Sorting Order On Page is required.");

            return Result.Ok();
        }
    }
}
