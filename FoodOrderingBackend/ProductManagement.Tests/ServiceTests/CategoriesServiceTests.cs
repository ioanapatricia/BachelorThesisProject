using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using ProductManagement.API.Entities;
using ProductManagement.API.Helpers.MapperProfiles;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Tests.TestData;
using Xunit;

namespace ProductManagement.Tests.ServiceTests
{
    public class CategoriesServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
        private readonly Mock<IProductsService> _mockedProductsService;
        private readonly Mock<IImagesService> _mockedImagesService;
        private readonly IMapper _mapper;
        private readonly ICategoriesService _categoriesService;
        private readonly CategoryTestData _categoryTestData;
        private readonly ProductTestData _productTestData;

        public CategoriesServiceTests()
        {
            _mockedUnitOfWork = new Mock<IUnitOfWork>();
            _mockedProductsService = new Mock<IProductsService>();
            _mockedImagesService = new Mock<IImagesService>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _categoriesService = new CategoriesService(_mockedUnitOfWork.Object, _mapper, _mockedProductsService.Object,
                _mockedImagesService.Object);
            _categoryTestData = new CategoryTestData();
            _productTestData = new ProductTestData();
        }

        [Fact]
        public async Task GetCategoryAsync_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductCategory());

            // Act  
            await _categoriesService.GetAsync(1);

            // Assert
            _mockedUnitOfWork.Verify(uow => uow.Categories.GetAsync(1), Times.Once);
        }




        [Fact]
        public async Task GetAllAsync_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAllCategoriesWithImagesAsync())
                .ReturnsAsync(new List<ProductCategory>());

            // Act  
            await _categoriesService.GetAllAsync();

            // Assert
            _mockedUnitOfWork.Verify(uow => uow.Categories.GetAllCategoriesWithImagesAsync(), Times.Once);
        }




        [Fact]
        public async Task GetAllCategoriesWithProducts_WhenCalled_CallsGetCategoriesInUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAllCategoriesWithImagesAsync())
                .ReturnsAsync(new List<ProductCategory>());
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act  
            await _categoriesService.GetAllCategoriesWithProductsAsync();

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Categories.GetAllCategoriesWithImagesAsync(), Times.Once);

        }

        [Fact]
        public async Task GetAllCategoriesWithProducts_WhenCalled_CallsGetProductsInUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAllCategoriesWithImagesAsync())
                .ReturnsAsync(new List<ProductCategory>());
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act  
            await _categoriesService.GetAllCategoriesWithProductsAsync();

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Products.GetAllAsync(), Times.Once);

        }

        [Fact]
        public async Task GetAllCategoriesWithProducts_IfThereIsNoCategory_ReturnsEmptyCategoryWithProductsList()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAllCategoriesWithImagesAsync())
                .ReturnsAsync(new List<ProductCategory>());
            var productsList = _productTestData.GetProducts();
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllAsync()).ReturnsAsync(productsList);


            // Act  
            var result = await _categoriesService.GetAllCategoriesWithProductsAsync();

            // Assert
            Assert.Empty(result);
        }


        [Fact]
        public async Task GetAllCategoriesWithProducts_IfThereAreNoProducts_ReturnsCategoryListWithNoProducts()
        {
            // Arrange  
            var categories = _categoryTestData.GetProductCategories();
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAllCategoriesWithImagesAsync()).ReturnsAsync(categories);
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllAsync()).ReturnsAsync(new List<Product>());


            // Act  
            var result = await _categoriesService.GetAllCategoriesWithProductsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, cat => Assert.Empty(cat.Products));
        }

        [Fact]
        public async Task GetAllCategoriesWithProducts_IfThereAreBothCategoriesAndProducts_ReturnsACorrectDto()
        {
            // Arrange  
            var categories = _categoryTestData.GetProductCategories();
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAllCategoriesWithImagesAsync()).ReturnsAsync(categories);

            var productsList = _productTestData.GetProducts();
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllAsync()).ReturnsAsync(productsList);

            // Act
            var result = await _categoriesService.GetAllCategoriesWithProductsAsync();

            Assert.NotEmpty(result);
            Assert.All(result, cat => Assert.NotEmpty(cat.Products));
            Assert.Equal(categories.Count(), result.Count());
        }













        [Fact]
        public async Task CreateCategoryAsync_WhenCalled_ProcessesImagesAndAddsCategoryInUnitOfWork()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Categories.Add(It.IsAny<ProductCategory>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            _mockedImagesService.Setup(s => s.GetProcessedImageWithExtension(It.IsAny<Image>())).Verifiable();

            var category = _categoryTestData.GetCategory(1);

            // Act      
            await _categoriesService.CreateCategoryAsync(category);

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Categories.Add(category), Times.Once);
            _mockedImagesService.Verify(x => x.GetProcessedImageWithExtension(It.IsAny<Image>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenCalled_CompletesTransaction()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Categories.Add(It.IsAny<ProductCategory>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            var category = _categoryTestData.GetCategory(1);

            // Act      
            await _categoriesService.CreateCategoryAsync(category);

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }


        [Fact]
        public async Task CreateCategoryAsync_IfCompletionFails_ReturnsFailedResult()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Categories.Add(It.IsAny<ProductCategory>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Fail(""));
            var category = _categoryTestData.GetCategory(1);

            var expectedResult = Result<ProductCategory>.Fail("");

            // Act      
            var result = await _categoriesService.CreateCategoryAsync(category);

            // Assert   
            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
        }


        [Fact]
        public async Task CreateCategoryAsync_IfCompletionIsSuccessful_ReturnsOkResultWithObject()
        {
            // Arrange  
            var category = _categoryTestData.GetCategory(1);

            _mockedUnitOfWork.Setup(uow => uow.Categories.Add(It.IsAny<ProductCategory>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Categories.GetAsync(It.IsAny<int>())).ReturnsAsync(category);
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            var expectedResult = Result<ProductCategory>.Ok(category);

            // Act      
            var result = await _categoriesService.CreateCategoryAsync(category);

            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.Equal(expectedResult.Value.Id, result.Value.Id);
        }








        [Fact]
        public async Task UpdateCategoryAsync_WhenCalled_DeletesOldImagesAndProcessesNewImages()
        {
            // Arrange  
            var category = _categoryTestData.GetCategory(1);
            var categoryDto = _categoryTestData.GetCategoryForCreateOrUpdateDto();


            _mockedImagesService.Setup(img => img.DeleteAsync(It.IsAny<int>())).Verifiable();
            _mockedImagesService.Setup(img => img.GetProcessedImageWithExtension(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            // Act          
            await _categoriesService.UpdateCategoryAsync(category, categoryDto);

            // Assert   
            _mockedImagesService.Verify(img => img.DeleteAsync(It.IsAny<int>()), Times.Exactly(2));
            _mockedImagesService.Verify(img => img.GetProcessedImageWithExtension(It.IsAny<Image>()), Times.Exactly(2));
            _mockedUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }




        [Fact]
        public async Task UpdateCategoryAsync_IfSuccessful_ReturnsOkResult()
        {
            // Arrange      
            var category = _categoryTestData.GetCategory(1);
            var categoryDto = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            _mockedUnitOfWork.Setup(uow => uow.Images.GetAsync(It.IsAny<int>())).ReturnsAsync(new Image());
            _mockedUnitOfWork.Setup(uow => uow.Images.GetAsync(It.IsAny<int>())).ReturnsAsync(new Image());


            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            var expectedResult = Result.Ok();

            // Act          
            var result = await _categoriesService.UpdateCategoryAsync(category, categoryDto);

            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
        }



        [Fact]
        public async Task UpdateCategoryAsync_IfFailed_ReturnsFailResult()
        {
            // Arrange      
            var category = _categoryTestData.GetCategory(1);
            var categoryDto = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            _mockedUnitOfWork.Setup(uow => uow.Images.GetAsync(It.IsAny<int>())).ReturnsAsync(new Image());
            _mockedUnitOfWork.Setup(uow => uow.Images.GetAsync(It.IsAny<int>())).ReturnsAsync(new Image());
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Fail(""));

            var expectedResult = Result.Fail("");

            // Act          
            var result = await _categoriesService.UpdateCategoryAsync(category, categoryDto);

            // Assert   
            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
        }














        [Fact]
        public async Task DeleteCategoryAsync_WhenCalled_CallsDeleteAllProductsInProductsService()
        {
            // Arrange      
            _mockedProductsService.Setup(ps => ps.DeleteAllProductsInCategoryAsync(It.IsAny<int>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Categories.Remove(It.IsAny<ProductCategory>())).Verifiable();

            var category = _categoryTestData.GetCategory(1);
            // Act
            await _categoriesService.DeleteCategoryAsync(category);

            // Assert   
            _mockedProductsService.Verify(ps => ps.DeleteAllProductsInCategoryAsync(1), Times.Once);
        }


        [Fact]
        public async Task DeleteCategoryAsync_WhenCalled_DeletesCategoryBannerAndLogo()
        {
            // Arrange      
            _mockedProductsService.Setup(ps => ps.DeleteAllProductsInCategoryAsync(It.IsAny<int>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Categories.Remove(It.IsAny<ProductCategory>())).Verifiable();
            _mockedImagesService.Setup(img => img.DeleteAsync(It.IsAny<int>())).Verifiable();

            var category = _categoryTestData.GetCategory(1);    
            // Act
            await _categoriesService.DeleteCategoryAsync(category); 

            // Assert   
            _mockedImagesService.Verify(img => img.DeleteAsync(It.IsAny<int>()), Times.Exactly(2));
        }


        [Fact]
        public async Task DeleteCategoryAsync_WhenCalled_RemovesCategoryAndCompletes()
        {
            // Arrange      
            _mockedProductsService.Setup(ps => ps.DeleteAllProductsInCategoryAsync(It.IsAny<int>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Categories.Remove(It.IsAny<ProductCategory>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).Verifiable();

            var category = _categoryTestData.GetCategory(1);
            // Act  
            await _categoriesService.DeleteCategoryAsync(category); 

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Categories.Remove(category), Times.Once);
            _mockedUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }


        [Fact]
        public async Task DeleteCategoryAsync_IfSuccessful_ReturnsOkResult()
        {
            // Arrange      
            _mockedProductsService.Setup(ps => ps.DeleteAllProductsInCategoryAsync(It.IsAny<int>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Categories.Remove(It.IsAny<ProductCategory>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            var category = _categoryTestData.GetCategory(1);

            // Act  
            var result = await _categoriesService.DeleteCategoryAsync(category);

            // Assert   
            Assert.True(result.Success);
        }


        [Fact]
        public async Task DeleteCategoryAsync_IfFailed_ReturnsFailResult()
        {
            // Arrange      
            _mockedProductsService.Setup(ps => ps.DeleteAllProductsInCategoryAsync(It.IsAny<int>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Categories.Remove(It.IsAny<ProductCategory>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Fail(""));
            var category = _categoryTestData.GetCategory(1);

            // Act  
            var result = await _categoriesService.DeleteCategoryAsync(category);

            // Assert   
            Assert.True(result.IsFailure);
        }
    }
}   
        