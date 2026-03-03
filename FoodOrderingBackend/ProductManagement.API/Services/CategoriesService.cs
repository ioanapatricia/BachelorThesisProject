using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Contracts;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace ProductManagement.API.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly IImagesService _imagesService;

        public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper, IProductsService productsService, IImagesService imagesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productsService = productsService;
            _imagesService = imagesService;
        }

        public async Task<ProductCategory> GetAsync(int id)
        {
            return await _unitOfWork.Categories.GetAsync(id);
        }

        public async Task<IEnumerable<ProductCategory>> GetAllAsync()
            => await _unitOfWork.Categories.GetAllCategoriesWithImagesAsync();


        public async Task<IEnumerable<CategoryWithProductsDto>> GetAllCategoriesWithProductsAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllCategoriesWithImagesAsync();    

            var products = await _unitOfWork.Products.GetAllAsync();

            var categoryWithProductsList = new List<CategoryWithProductsDto>();

            foreach (var category in categories)
            {
                var productsWithinThisCategory = products.Where(prd => prd.CategoryId == category.Id);

                categoryWithProductsList.Add(ProcessCategoryWithProduct(category, productsWithinThisCategory));
            }

            return categoryWithProductsList;
        }

        public async Task<Result<ProductCategory>> CreateCategoryAsync(ProductCategory category)
        {
            category.Banner = _imagesService.GetProcessedImageWithExtension(category.Banner);
            category.Logo = _imagesService.GetProcessedImageWithExtension(category.Logo);

            _unitOfWork.Categories.Add(category);

            var categoryCreationResult = await _unitOfWork.CompleteAsync();

            return categoryCreationResult.IsFailure
                ? Result<ProductCategory>.Fail(categoryCreationResult.Error)
                : Result<ProductCategory>.Ok(await _unitOfWork.Categories.GetAsync(category.Id));
        }

        public async Task<Result> UpdateCategoryAsync(ProductCategory category, ProductCategoryForCreateOrUpdateDto categoryDto)
        {
            await DeleteCategoryImagesAsync(category);

            _mapper.Map(categoryDto, category);

            category.Logo = _imagesService.GetProcessedImageWithExtension(category.Logo);

            category.Banner = _imagesService.GetProcessedImageWithExtension(category.Banner);

            return await _unitOfWork.CompleteAsync();
        }


        public async Task<Result> DeleteCategoryAsync(ProductCategory category)
        {
            await _productsService.DeleteAllProductsInCategoryAsync(category.Id);

            await DeleteCategoryImagesAsync(category);

            _unitOfWork.Categories.Remove(category);

            var result =  await _unitOfWork.CompleteAsync();

            return result;
        }


        private async Task DeleteCategoryImagesAsync(ProductCategory category)
        {
            await _imagesService.DeleteAsync(category.Logo.Id);
            await _imagesService.DeleteAsync(category.Banner.Id);
        }

        private CategoryWithProductsDto ProcessCategoryWithProduct(ProductCategory productCategory,
            IEnumerable<Product> productsWithinThisCategory)
        {
            return new()
            {   
                Name = productCategory.Name,
                Logo = _mapper.Map<ImageForGetDto>(productCategory.Logo),
                Banner = _mapper.Map<ImageForGetDto>(productCategory.Banner),
                SortingOrderOnWebpage = productCategory.SortingOrderOnWebpage,
                Products = _mapper.Map<IEnumerable<ProductForGetDto>>(productsWithinThisCategory)
            };
        }
    }
}   
