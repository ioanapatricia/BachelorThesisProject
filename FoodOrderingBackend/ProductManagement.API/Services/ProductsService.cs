using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc;
using Ordering.Contracts.Models;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Services
{
    public class ProductsService : ControllerBase, IProductsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImagesService _imagesService;

        public ProductsService(IUnitOfWork unitOfWork, IMapper mapper, IImagesService imagesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imagesService = imagesService;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _unitOfWork.Products.GetAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<Result<IEnumerable<Product>>> GetProductsByFilterAsync(IEnumerable<ProductFilter> productFilters)
        {
            var products = await _unitOfWork.Products.GetAllByFilterAsync(productFilters);

            if(!products.Any())
                return Result<IEnumerable<Product>>.Ok(products);

            return IsFilteredProductListValid(products)
                ? Result<IEnumerable<Product>>.Ok(products)
                : Result<IEnumerable<Product>>.Fail("Broken entries in PM.API");
        }


        public async Task<Result<Product>> CreateProductAsync(Product product)
        {
            product.Image = _imagesService.GetProcessedImageWithExtension(product.Image);

            _unitOfWork.Products.Add(product);

            var productCreationResult = await _unitOfWork.CompleteAsync();

            return productCreationResult.IsFailure 
                ? Result<Product>.Fail(productCreationResult.Error) 
                : Result<Product>.Ok(await _unitOfWork.Products.GetAsync(product.Id));
        }



        public async Task<Result> UpdateProductAsync(Product product, ProductForCreateOrUpdateDto productDto)
        {
            await DeleteProductDependencies(product);

            _mapper.Map(productDto, product);

            product.Image = _imagesService.GetProcessedImageWithExtension(product.Image);

            var productUpdateResult = await _unitOfWork.CompleteAsync();

            return productUpdateResult.IsFailure ? Result.Fail(productUpdateResult.Error) : Result.Ok();
        }

        public async Task<Result> DeleteProductAsync(Product product)
        {
            await DeleteProductDependencies(product);
            _unitOfWork.Products.Remove(product);

            var productDeleteResult = await _unitOfWork.CompleteAsync();

            return productDeleteResult.Success ? Result.Ok() : Result.Fail(productDeleteResult.Error);
        }

        public async Task DeleteAllProductsInCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetAllProductsByCategoryAsync(categoryId);
            foreach (var product in products)
            {
                await DeleteProductDependencies(product);
                _unitOfWork.Products.Remove(product);
            }   
        }

        private async Task DeleteProductDependencies(Product product)
        {
            var variants = await _unitOfWork.Variants
                .FindAsync(v => v.ProductId == product.Id);
            _unitOfWork.Variants.RemoveRange(variants);

            await _imagesService.DeleteAsync(product.Image.Id);
        }


        private static bool IsFilteredProductListValid(IEnumerable<Product> products)
            => products.All(product => product.Variants.Count == 1);
    }
}
    