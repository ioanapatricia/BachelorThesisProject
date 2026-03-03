using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagement.API.Controllers;
using ProductManagement.API.Entities;
using ProductManagement.API.Helpers.MapperProfiles;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;
using ProductManagement.Tests.TestData;
using Xunit;

namespace ProductManagement.Tests.ControllerTests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoriesService> _mockedCategoriesService;
        private readonly Mock<ICategoryValidator> _mockedCategoryValidator;
        private readonly IMapper _mapper;
        private readonly CategoriesController _categoriesController;
        private readonly CategoryTestData _categoryTestData;
        

        public CategoriesControllerTests()
        {
            _mockedCategoriesService = new Mock<ICategoriesService>();
            _mockedCategoryValidator = new Mock<ICategoryValidator>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);
            _categoriesController = new CategoriesController(_mockedCategoriesService.Object, _mapper, _mockedCategoryValidator.Object);

            _categoryTestData = new CategoryTestData();
        }

        [Fact]
        public async Task GetCategory_WhenCalled_CallsCategoriesService()
        {
            // Act  
            await _categoriesController.GetCategory(1);

            // Assert
            _mockedCategoriesService.Verify(s => s.GetAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetCategory_IfCategoryExists_ReturnsOkWithCategory()
        {
            // Arrange
            var category = _categoryTestData.GetCategory(1);

            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(category);

            // Act  
            var result = await _categoriesController.GetCategory(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as ProductCategoryForGetDto;
            Assert.NotNull(responseBody);
            Assert.Equal(responseBody.Id, category.Id);
        }


        [Fact]
        public async Task GetCategory_IfCategoryDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((Func<ProductCategory>)null);

            // Act  
            var actualResult = await _categoriesController.GetCategory(1);

            // Assert
            actualResult.Should().BeOfType<NotFoundObjectResult>();
        }











        [Fact]
        public async Task GetAll_WhenCalled_CallsCategoriesService()
        {
            // Act
            await _categoriesController.GetAll();

            // Assert
            _mockedCategoriesService.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAll_IfThereIsNoCategory_ReturnsNoContent()
        {
            // Arrange
            _mockedCategoriesService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ProductCategory>());

            // Act
            var result = await _categoriesController.GetAll();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetAll_IfThereIsOneOrMoreCategories_ReturnsOkWithCategoriesList()
        {
            // Arrange
            var categories = _categoryTestData.GetProductCategories();

            _mockedCategoriesService.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoriesController.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as IEnumerable<ProductCategoryForListDto>;
            Assert.NotNull(responseBody);


            Assert.All(responseBody, productCategoryForListDto =>
                Assert.Equal(categories.First(p => p.Id == productCategoryForListDto.Id).Id,
                    productCategoryForListDto.Id));
        }












        [Fact]
        public async Task GetAllWithProducts_WhenCalled_CallsCategoriesService()
        {
            // Act
            await _categoriesController.GetAllWithProducts();

            // Assert
            _mockedCategoriesService.Verify(s => s.GetAllCategoriesWithProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllWithProducts_IfThereIsNoCategory_ReturnsNoContent()
        {
            // Arrange
            _mockedCategoriesService.Setup(s => s.GetAllCategoriesWithProductsAsync()).ReturnsAsync(new List<CategoryWithProductsDto>());

            // Act
            var result = await _categoriesController.GetAllWithProducts();

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetAllWithProducts_IfThereIsOneOrMoreCategories_ReturnsOkWithCategoriesList()
        {
            // Arrange
            var categories = _categoryTestData.GetCategoryWithProducts();

            _mockedCategoriesService.Setup(s => s.GetAllCategoriesWithProductsAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoriesController.GetAllWithProducts();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as IEnumerable<CategoryWithProductsDto>;
            Assert.NotNull(responseBody);


            Assert.All(responseBody, categoryWithProductsDto =>
                Assert.Equal(categories.First(p => p.Name == categoryWithProductsDto.Name).Name,
                    categoryWithProductsDto.Name));
        }












        [Fact]
        public async Task CreateCategory_WhenCalled_CallsValidateCategoryInService()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);
            _mockedCategoriesService.Setup(s => s.CreateCategoryAsync(It.IsAny<ProductCategory>())).ReturnsAsync(Result<ProductCategory>.Ok(new ProductCategory()));

            var categoryForCreate = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            // Act  
            await _categoriesController.Create(categoryForCreate);

            // Assert
            _mockedCategoryValidator.Verify(categoryValidator => categoryValidator.ValidateCategory(categoryForCreate), Times.Once);
        }

        [Fact]
        public async Task CreateCategory_IfValidationFails_ReturnsBadRequest()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Fail(""));

            var categoryForCreate = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            // Act  
            var result = await _categoriesController.Create(categoryForCreate);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCategory_IfValidationFails_DoesNotCallCreateCategoryInService()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Fail("validation error"));
            _mockedCategoriesService.Setup(s => s.CreateCategoryAsync(It.IsAny<ProductCategory>())).ReturnsAsync(Result<ProductCategory>.Ok(new ProductCategory()));

            var categoryForCreate = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            // Act  
            await _categoriesController.Create(categoryForCreate);

            // Assert
            _mockedCategoryValidator.Verify(categoryValidator => categoryValidator.ValidateCategory(categoryForCreate), Times.Once);
            _mockedCategoriesService.Verify(categoryService => categoryService.CreateCategoryAsync(It.IsAny<ProductCategory>()), Times.Never);
        }

        [Fact]
        public async Task CreateCategory_IfValidationIsSuccessful_CallsCreateCategoryInService()
        {
            // Arrange  
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);
            _mockedCategoriesService.Setup(s => s.CreateCategoryAsync(It.IsAny<ProductCategory>())).ReturnsAsync(Result<ProductCategory>.Ok(new ProductCategory()));

            // Act  
            await _categoriesController.Create(new ProductCategoryForCreateOrUpdateDto());

            // Assert
            _mockedCategoriesService.Verify(categoryService => categoryService.CreateCategoryAsync(It.IsAny<ProductCategory>()), Times.Once);
        }

        [Fact]
        public async Task CreateCategory_IfCategoryIsSuccessfullyCreated_ReturnsCreatedAtRoute()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);

            var category = _categoryTestData.GetCategory(1);
            _mockedCategoriesService.Setup(s => s.CreateCategoryAsync(It.IsAny<ProductCategory>())).ReturnsAsync(Result<ProductCategory>.Ok(category));

            var categoryForCreateDto = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            // Act  
            var result = await _categoriesController.Create(categoryForCreateDto);


            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();

            var response = (CreatedAtRouteResult)result;
            var responseBody = response.Value as ProductCategoryForGetDto;

            Assert.NotNull(responseBody);
            Assert.Equal(responseBody.Id, category.Id);
        }


        [Fact]
        public async Task CreateCategory_IfCreationFailsInService_ReturnsInternalServerError()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);
            _mockedCategoriesService.Setup(s => s.CreateCategoryAsync(It.IsAny<ProductCategory>())).ReturnsAsync(Result<ProductCategory>.Fail("error lol"));
            // Act  
            var result = await _categoriesController.Create(new ProductCategoryForCreateOrUpdateDto());

            // Assert
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }




















        [Fact]
        public async Task UpdateCategory_WhenCalled_CallsValidateCategoryInService()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);
            

            var categoryForUpdate = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            // Act  
            await _categoriesController.Update(1, categoryForUpdate);

            // Assert
            _mockedCategoryValidator.Verify(categoryValidator => categoryValidator.ValidateCategory(categoryForUpdate), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_IfValidationFails_ReturnsBadRequest()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Fail(""));

            var categoryForUpdate = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            // Act  
            var result = await _categoriesController.Update(1, categoryForUpdate);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
            
        [Fact]
        public async Task UpdateCategory_IfValidationFails_DoesNotCallUpdateCategoryInService()
        {
            // Arrange
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Fail("validation error"));
            _mockedCategoriesService.Setup(s => s.UpdateCategoryAsync(It.IsAny<ProductCategory>(), It.IsAny<ProductCategoryForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok());

            var categoryForUpdate = _categoryTestData.GetCategoryForCreateOrUpdateDto();

            // Act  
            await _categoriesController.Update(1, categoryForUpdate);

            // Assert
            _mockedCategoryValidator.Verify(categoryValidator => categoryValidator.ValidateCategory(categoryForUpdate), Times.Once);
            _mockedCategoriesService.Verify(categoryService => categoryService.UpdateCategoryAsync(It.IsAny<ProductCategory>(),It.IsAny<ProductCategoryForCreateOrUpdateDto>()), Times.Never);
        }

        [Fact]
        public async Task UpdateCategory_IfValidationIsSuccessful_CallsGetCategoryInService()
        {
            // Arrange  
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);

            // Act  
            await _categoriesController.Update(1, new ProductCategoryForCreateOrUpdateDto());

            // Assert
            _mockedCategoriesService.Verify(s => s.GetAsync(1), Times.Once);
        }


        [Fact]
        public async Task UpdateCategory_IfCategoryFromServiceIsNull_ReturnsNotFound()
        {
            // Arrange  
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);

            // Act  
            var result = await _categoriesController.Update(1, new ProductCategoryForCreateOrUpdateDto());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Fact]
        public async Task UpdateCategory_IfCategoryFromServiceIsNotNull_CallsUpdateCategoryInService()
        {
            // Arrange  
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);
            _mockedCategoriesService.Setup(s => s.UpdateCategoryAsync(It.IsAny<ProductCategory>(), It.IsAny<ProductCategoryForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok());
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(new ProductCategory());

            // Act  
            await _categoriesController.Update(1, new ProductCategoryForCreateOrUpdateDto());

            // Assert
            _mockedCategoriesService.Verify(categoryService => categoryService.UpdateCategoryAsync(It.IsAny<ProductCategory>(), It.IsAny<ProductCategoryForCreateOrUpdateDto>()), Times.Once);
        }


        [Fact]
        public async Task UpdateCategory_IfUpdateFailsInService_ReturnsInternalServerError()
        {
            // Arrange  
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);
            _mockedCategoriesService.Setup(s => s.UpdateCategoryAsync(It.IsAny<ProductCategory>(), It.IsAny<ProductCategoryForCreateOrUpdateDto>())).ReturnsAsync(Result.Fail(""));
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(new ProductCategory());

            // Act  
            var result =  await _categoriesController.Update(1, new ProductCategoryForCreateOrUpdateDto());

            // Assert
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }


        [Fact]
        public async Task UpdateCategory_IfUpdateIsSuccessful_ReturnsNoContent()
        {
            // Arrange  
            _mockedCategoryValidator.Setup(cv => cv.ValidateCategory(It.IsAny<ProductCategoryForCreateOrUpdateDto>())).Returns(Result.Ok);
            _mockedCategoriesService.Setup(s => s.UpdateCategoryAsync(It.IsAny<ProductCategory>(), It.IsAny<ProductCategoryForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok());
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(new ProductCategory());

            // Act  
            var result = await _categoriesController.Update(1, new ProductCategoryForCreateOrUpdateDto());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }










        [Fact]
        public async Task DeleteCategory_WhenCalled_CallsGetInCategoriesService()
        {
            // Act      
            await _categoriesController.Delete(1);

            // Assert
            _mockedCategoriesService.Verify(s => s.GetAsync(1), Times.Once);
        }


        [Fact]
        public async Task DeleteCategory_IfCategoryDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync((Func<ProductCategory>)null);

            // Act          
            var result =  await _categoriesController.Delete(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Fact]  
        public async Task DeleteCategory_IfCategoryExists_CallsDeleteInService()
        {
            // Arrange
            var category = new ProductCategory();
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(category);
            _mockedCategoriesService.Setup(s => s.DeleteCategoryAsync(It.IsAny<ProductCategory>()))
                .ReturnsAsync(Result.Ok);

            // Act          
            await _categoriesController.Delete(1);

            // Assert
            _mockedCategoriesService.Verify(s => s.DeleteCategoryAsync(category), Times.Once);
        }


        [Fact]  
        public async Task DeleteCategory_IfDeleteIsSuccessful_ReturnsAllGucci()
        {
            // Arrange
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(new ProductCategory());
            _mockedCategoriesService.Setup(s => s.DeleteCategoryAsync(It.IsAny<ProductCategory>()))
                .ReturnsAsync(Result.Ok);

            // Act          
            var result = await _categoriesController.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }



        [Fact]
        public async Task DeleteCategory_IfDeleteFailed_ReturnsInternalServerError()
        {
            // Arrange
            _mockedCategoriesService.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(new ProductCategory());
            _mockedCategoriesService.Setup(s => s.DeleteCategoryAsync(It.IsAny<ProductCategory>()))
                .ReturnsAsync(Result.Fail(""));

            // Act          
            var result = await _categoriesController.Delete(1);

            // Assert   
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }

    }
}
