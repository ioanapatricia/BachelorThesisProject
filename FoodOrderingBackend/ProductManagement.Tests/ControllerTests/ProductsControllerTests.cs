using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ordering.Contracts.Models;
using ProductManagement.API.Controllers;
using ProductManagement.API.Entities;
using ProductManagement.API.Helpers.MapperProfiles;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Tests.TestData;
using Xunit;

namespace ProductManagement.Tests.ControllerTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductsService> _mockedProductsService;
        private readonly Mock<IProductValidator> _mockedProductValidator;
        private readonly ProductsController _productsController;
        private readonly IMapper _mapper;
        private readonly ProductTestData _productTestData;
        public ProductsControllerTests()    
        {
            _mockedProductsService = new Mock<IProductsService>();
            _mockedProductValidator = new Mock<IProductValidator>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _productsController = new ProductsController(_mockedProductsService.Object, _mockedProductValidator.Object,
                _mapper);

            _productTestData = new ProductTestData();
        }


        [Fact]
        public async Task GetProduct_WhenCalled_CallsProductsService()
        {
            // Act  
           await _productsController.GetProduct(1); 

            // Assert
            _mockedProductsService.Verify(s => s.GetProductAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetProduct_IfProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            var product = _productTestData.GetProduct(1);

            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(product);

            // Act  
            var result = await _productsController.GetProduct(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult) result;
            var responseBody = response.Value as ProductForGetDto;
            Assert.NotNull(responseBody);
            Assert.Equal(responseBody.Id, product.Id);
        }


        [Fact]
        public async Task GetProduct_IfProductDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync((Func<Product>) null);

            // Act  
            var actualResult = await _productsController.GetProduct(1);

            // Assert
            actualResult.Should().BeOfType<NotFoundObjectResult>();
        }





        [Fact]
        public async Task GetProducts_WhenCalled_CallsProductsService()
        {
            // Act      
            await _productsController.GetProducts();

            // Assert
            _mockedProductsService.Verify(s => s.GetProductsAsync(), Times.Once);
        }


        [Fact]
        public async Task GetProducts_IfThereIsOneOrMoreProducts_ReturnsOkWithProductList()
        {
            // Arrange  
            var products = _productTestData.GetProducts();

            _mockedProductsService.Setup(s => s.GetProductsAsync()).ReturnsAsync(products);

            // Act  
            var result = await _productsController.GetProducts();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as IEnumerable<ProductForGetDto>;
            Assert.NotNull(responseBody);


            Assert.All(responseBody, responseProductForGet =>
                Assert.Equal(products.First(p => p.Id == responseProductForGet.Id).Id, 
                    responseProductForGet.Id));
        }




        [Fact]
        public async Task GetProducts_IfNoProductExists_ReturnsNoContent()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductsAsync()).ReturnsAsync(new List<Product>());

            // Act  
            var actualResult = await _productsController.GetProducts();

            // Assert
            actualResult.Should().BeOfType<NoContentResult>();  
        }















        [Fact]
        public async Task GetProductsByFilter_WhenCalled_CallsProductsService()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductsByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(new Result<IEnumerable<Product>>());

            // Act      
            await _productsController.GetProductsByFilter(new List<ProductFilter>());

            // Assert
            _mockedProductsService.Verify(s => s.GetProductsByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()), Times.Once);
        }

        [Fact]
        public async Task GetProductsByFilter_IfIsFailed_ReturnsInternalServerError()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductsByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(Result<IEnumerable<Product>>.Fail("error lol"));

            // Act      
            var result = await _productsController.GetProductsByFilter(new List<ProductFilter>());

            // Assert
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }



            
        [Fact]
        public async Task GetProductsByFilter_IfThereAreNoProducts_ReturnsNotFound()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductsByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(Result<IEnumerable<Product>>.Ok(new List<Product>()));

            // Act      
            var result = await _productsController.GetProductsByFilter(new List<ProductFilter>());

            // Assert

            result.Should().BeOfType<NotFoundResult>();    
        }


        [Fact]
        public async Task GetProductsByFilter_IfThereIsAtLeastOneProduct_ReturnsOkWithProductList()
        {
            // Arrange
            var products = _productTestData.GetProducts();
            _mockedProductsService.Setup(s => s.GetProductsByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(Result<IEnumerable<Product>>.Ok(products));
                
            // Act      
            var result = await _productsController.GetProductsByFilter(new List<ProductFilter>());

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as IEnumerable<ProductForGetDto>;
            Assert.NotNull(responseBody);


            Assert.All(responseBody, responseProductForGet =>
                Assert.Equal(products.First(p => p.Id == responseProductForGet.Id).Id,
                    responseProductForGet.Id));
        }










        [Fact]
        public async Task CreateProduct_WhenCalled_CallsValidateProductInService()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(Result<Product>.Ok(new Product()));

            var productForCreateOrUpdate = _productTestData.GetProductForCreateOrUpdateDto();

            // Act  
            await _productsController.Create(productForCreateOrUpdate);

            // Assert
            _mockedProductValidator.Verify(productValidator => productValidator.ValidateProduct(productForCreateOrUpdate), Times.Once);
        }

        [Fact]
        public async Task CreateProduct_IfValidationFails_ReturnsBadRequest()   
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Fail(""));

            var productForCreateOrUpdate = _productTestData.GetProductForCreateOrUpdateDto();

            // Act  
            var result =  await _productsController.Create(productForCreateOrUpdate);
            
            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]
        public async Task CreateProduct_IfValidationFails_DoesNotCallCreateProductInService()    
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Fail("validation error"));
            _mockedProductsService.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(Result<Product>.Ok(new Product()));

            var productForCreateOrUpdate = _productTestData.GetProductForCreateOrUpdateDto();
            // Act  
            await _productsController.Create(productForCreateOrUpdate);

            // Assert
            _mockedProductValidator.Verify(productValidator => productValidator.ValidateProduct(productForCreateOrUpdate), Times.Once);
            _mockedProductsService.Verify(productService => productService.CreateProductAsync(It.IsAny<Product>()), Times.Never);
        }
            
            
        [Fact]  
        public async Task CreateProduct_IfValidationIsSuccessful_CallsCreateProductInService()
        {
            // Arrange  
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(Result<Product>.Ok(new Product()));

            // Act  
            await _productsController.Create(new ProductForCreateOrUpdateDto());    

            // Assert
            _mockedProductsService.Verify(productService => productService.CreateProductAsync(It.IsAny<Product>()), Times.Once);
        }


        [Fact]
        public async Task CreateProduct_IfProductIsSuccessfullyCreated_ReturnsCreatedAtRoute()      
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);

            var product = _productTestData.GetProduct(1);
            _mockedProductsService.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(Result<Product>.Ok(product));

            var productForCreateOrUpdateDto = _productTestData.GetProductForCreateOrUpdateDto();

            // Act  
            var result = await _productsController.Create(productForCreateOrUpdateDto);


            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();

            var response = (CreatedAtRouteResult)result;
            var responseBody = response.Value as ProductForGetDto;

            Assert.NotNull(responseBody);
            Assert.Equal(responseBody.Id, product.Id);
        }   

            
        [Fact]
        public async Task CreateProduct_IfCreationFailsInService_ReturnsInternalServerError()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(Result<Product>.Fail("error lol"));
            // Act  
            var result = await _productsController.Create(new ProductForCreateOrUpdateDto());

            // Assert
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }





            


            
        [Fact]
        public async Task UpdateProduct_WhenCalled_CallsValidateProduct()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockedProductsService
                .Setup(productsService =>
                    productsService.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()))
                .ReturnsAsync(Result.Ok);
                
            var productForCreateOrUpdate = _productTestData.GetProductForCreateOrUpdateDto();

            // Act  
            await _productsController.Update(1, productForCreateOrUpdate);

            // Assert
            _mockedProductValidator.Verify(productValidator => productValidator.ValidateProduct(productForCreateOrUpdate), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_IfValidationFails_ReturnsBadRequest()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Fail(""));

            // Act  
            var result = await _productsController.Update(1, new ProductForCreateOrUpdateDto());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]  
        public async Task UpdateProduct_IfValidationFails_DoesNotCallAnyServiceMethod()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Fail(""));
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockedProductsService
                .Setup(productsService =>
                    productsService.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()))
                .ReturnsAsync(Result.Ok);

            // Act  
            await _productsController.Update(1, new ProductForCreateOrUpdateDto());

            // Assert
            _mockedProductsService.Verify(s => s.GetProductAsync(It.IsAny<int>()), Times.Never);
            _mockedProductsService.Verify(s => s.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()), Times.Never);
        }


            
            
        [Fact]
        public async Task UpdateProduct_IfValidationIsGucciButProductIsNotFound_DoesNotCallUpdateProductInService()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync((Func<Product>)null);
            _mockedProductsService
                .Setup(productsService =>
                    productsService.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()))
                .ReturnsAsync(Result.Fail(""));

            // Act  
            await _productsController.Update(1, new ProductForCreateOrUpdateDto());

            // Assert
            _mockedProductsService.Verify(s => s.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()), Times.Never);
        }


        [Fact]
        public async Task UpdateProduct_IfValidationIsGucciButProductIsNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync((Func<Product>)null);
            _mockedProductsService
                .Setup(productsService =>
                    productsService.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()))
                .ReturnsAsync(Result.Fail(""));

            // Act  
            var result = await _productsController.Update(1, new ProductForCreateOrUpdateDto());

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Fact]      
        public async Task UpdateProduct_IfValidationIsGucciAndProductIsFound_CallsUpdateProductInService()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockedProductsService  
                .Setup(productsService =>
                    productsService.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()))
                .ReturnsAsync(Result.Ok);

            var productForCreateOrUpdate = _productTestData.GetProductForCreateOrUpdateDto();

            // Act  
            await _productsController.Update(1, productForCreateOrUpdate);

            // Assert
            _mockedProductsService.Verify(s => s.UpdateProductAsync(It.IsAny<Product>(), productForCreateOrUpdate), Times.Once);
        }



        [Fact]
        public async Task UpdateProduct_IfServiceCouldNotUpdate_ReturnsInternalServerError()  
        {   
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockedProductsService
                .Setup(productsService =>
                    productsService.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()))
                .ReturnsAsync(Result.Fail("error"));

            // Act  
            var result = await _productsController.Update(1, new ProductForCreateOrUpdateDto());

            // Assert

            result.Should().BeOfType<ObjectResult>();
                
            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }


        [Fact]  
        public async Task UpdateProduct_AllGucci_ReturnsNoContent()
        {
            // Arrange
            _mockedProductValidator.Setup(pv => pv.ValidateProduct(It.IsAny<ProductForCreateOrUpdateDto>())).ReturnsAsync(Result.Ok);
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockedProductsService
                .Setup(productsService =>
                    productsService.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<ProductForCreateOrUpdateDto>()))
                .ReturnsAsync(Result.Ok);

            // Act  
            var result = await _productsController.Update(1, new ProductForCreateOrUpdateDto());

            // Assert

            result.Should().BeOfType<NoContentResult>();
        }










            
        [Fact]  
        public async Task DeleteProduct_WhenCalled_CallsGetProductInService()
        {
            // Act  
            await _productsController.Delete(1);

            // Assert
            _mockedProductsService.Verify(s => s.GetProductAsync(1), Times.Once);
        }


        [Fact]  
        public async Task DeleteProduct_IfProductIsFound_CallsDeleteProductInService()
        {
            // Arrange
            var product = _productTestData.GetProduct(1);

            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(product);
            _mockedProductsService.Setup(s => s.DeleteProductAsync(It.IsAny<Product>())).ReturnsAsync(Result.Ok);

            // Act  
            await _productsController.Delete(1);

            // Assert
            _mockedProductsService.Verify(s => s.DeleteProductAsync(product), Times.Once);
        }


        [Fact]
        public async Task DeleteProduct_IfProductIsNotFound_DoesNotCallDeleteProductInService()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync((Func<Product>)null);
            _mockedProductsService.Setup(s => s.DeleteProductAsync(It.IsAny<Product>())).ReturnsAsync(Result.Fail(""));

            // Act  
            await _productsController.Delete(1);

            // Assert
            _mockedProductsService.Verify(s => s.DeleteProductAsync(It.IsAny<Product>()), Times.Never);
        }

            
        [Fact]
        public async Task DeleteProduct_IfProductIsNotFound_ReturnsNotFound()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync((Func<Product>)null);
            _mockedProductsService.Setup(s => s.DeleteProductAsync(It.IsAny<Product>())).ReturnsAsync(Result.Fail(""));

            // Act  
            var result = await _productsController.Delete(1);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }



        [Fact]
        public async Task DeleteProduct_IfDeletionIsSuccessful_ReturnsNoContent()
        {
            // Arrange
            _mockedProductsService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            _mockedProductsService.Setup(s => s.DeleteProductAsync(It.IsAny<Product>())).ReturnsAsync(Result.Ok);

            // Act  
            var result = await _productsController.Delete(1);

            // Assert   
            result.Should().BeOfType<NoContentResult>();    
        }   
    }
}   
                    