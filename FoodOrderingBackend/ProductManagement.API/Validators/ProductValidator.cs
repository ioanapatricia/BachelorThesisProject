using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.Persistence;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Validators
{
    public class ProductValidator : ControllerBase, IProductValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageValidator _imageValidator;

        public ProductValidator(IUnitOfWork unitOfWork, IImageValidator imageValidator)
        {
            _unitOfWork = unitOfWork;
            _imageValidator = imageValidator;
        }

        public async Task<Result> ValidateProduct(ProductForCreateOrUpdateDto product)
        {
            var productVariantsValidationResult = ValidateVariants(product.Variants);
            if (productVariantsValidationResult.IsFailure)
                return productVariantsValidationResult;

            //if (formFileCollection.Count == 0)
            //    return Result.Fail("Image is required.");

            var productImageValidationResult = _imageValidator.ValidateImage(product.Image);
            if (productImageValidationResult.IsFailure)
                return productImageValidationResult;

            var productPrimitivesValidationResult = ValidateProductPrimitives(product);
            if (productPrimitivesValidationResult.IsFailure)
                return productPrimitivesValidationResult;

            var validProductDependenciesResult = await AreProductDependenciesValid(product.CategoryId,
                product.WeightTypeId);

            if (validProductDependenciesResult.IsFailure)
                return validProductDependenciesResult;

            return Result.Ok();
        }

        private Result ValidateVariants(ICollection<ProductVariantForCreateDto> variants)
        {
            if (variants.Count == 0)
                return Result.Fail("At least one variant is required.");

            foreach (var variant in variants)
            {
                var variantPrimitivesValidationResult = ValidateVariantPrimitives(variant);
                if (variantPrimitivesValidationResult.IsFailure)
                    return Result.Fail(variantPrimitivesValidationResult.Error);
            }

            return Result.Ok();
        }

        private Result ValidateVariantPrimitives(ProductVariantForCreateDto variants)
        {
            if (string.IsNullOrEmpty(variants.Name))
                return Result.Fail("Variant name is required.");

            if (variants.Name.Length < 3 || variants.Name.Length > 30)
                return Result.Fail("Variant name should be between 3 and 30 characters.");

            if(variants.Price == default)
                return Result.Fail("Price is required.");

            if(variants.Weight == default)
                return Result.Fail("Weight is required.");

            return Result.Ok();
        }

        private Result ValidateProductPrimitives(ProductForCreateOrUpdateDto product)
        {
            if (string.IsNullOrEmpty(product.Name))
                return Result.Fail("Name is required.");

            if (product.Name.Length < 3 || product.Name.Length > 30)
                return Result.Fail("Name should be between 3 and 30 characters.");

            if(string.IsNullOrEmpty(product.Description))
                return Result.Fail("Description is required.");

            if(product.Description.Length < 20 || product.Description.Length > 500)
                return Result.Fail("Description should be between 20 and 500 characters.");

            if(!product.CategoryId.HasValue)
                return Result.Fail("Category is required");

            if(!product.WeightTypeId.HasValue)
                return Result.Fail("Weight Type is required");


            return Result.Ok();
        }

        private async Task<Result> AreProductDependenciesValid(int? categoryId, int? weightTypeId)
        {
            if (categoryId.HasValue)
            {
                var chosenCategory = await _unitOfWork.Categories.GetAsync((int)categoryId);

                if (chosenCategory == null)
                    return Result.Fail($"The product category with id {categoryId} does not exist");
            }

            if (weightTypeId.HasValue)
            {
                var chosenWeightType = await _unitOfWork.WeightTypes.GetAsync((int)weightTypeId);

                if (chosenWeightType == null)
                    return Result.Fail($"The weight type with id {weightTypeId} does not exist");
            }

            return Result.Ok();
        }
    }
}
    