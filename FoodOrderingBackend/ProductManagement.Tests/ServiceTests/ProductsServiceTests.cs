using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Contracts;
using Moq;
using Ordering.Contracts.Models;
using ProductManagement.API.Entities;
using ProductManagement.API.Helpers.MapperProfiles;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.Tests.TestData;
using Xunit;

namespace ProductManagement.Tests.ServiceTests
{
    public class ProductsServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductsService _productsService;
        private readonly Mock<IImagesService> _mockedImagesService;
        private readonly ProductTestData _productTestData;
        public ProductsServiceTests()
        {
            _mockedUnitOfWork = new Mock<IUnitOfWork>();
            _mockedImagesService = new Mock<IImagesService>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _productsService = new ProductsService(_mockedUnitOfWork.Object, _mapper, _mockedImagesService.Object);
            _productTestData = new ProductTestData();
        }



        [Fact]
        public async Task GetProductAsync_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAsync(It.IsAny<int>())).ReturnsAsync(new Product());

            // Act  
            await _productsService.GetProductAsync(1);

            // Assert
            _mockedUnitOfWork.Verify(uow => uow.Products.GetAsync(1), Times.Once);
        }









        [Fact]
        public async Task GetProductsAsync_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllAsync()).ReturnsAsync(new List<Product>());

            // Act  
            await _productsService.GetProductsAsync();

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Products.GetAllAsync(), Times.Once);
        }











        [Fact]
        public async Task GetProductsByFilterAsync_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(new List<Product>());

            // Act      
            await _productsService.GetProductsByFilterAsync(new List<ProductFilter>());

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Products.GetAllByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()), Times.Once);
        }

        [Fact]
        public async Task GetProductsByFilterAsync_IfThereAreNoProducts_ReturnsOkWithEmptyList()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(new List<Product>());
            var expectedResult = Result<List<Product>>.Ok(new List<Product>());

            // Act      
            var result = await _productsService.GetProductsByFilterAsync(new List<ProductFilter>());



            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.Empty(result.Value);
        }



        [Fact]
        public async Task GetProductsByFilterAsync_IfAnyProductHasMoreOreLessThanOneVariant_ReturnsFailedResult()
        {
            // Arrange  
            var products = _productTestData.GetProducts();
            var expectedResult = Result<IEnumerable<Product>>.Fail("Bubuit-am bubuit, ziurel de noapte");

            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(products);

            // Act      
            var result = await _productsService.GetProductsByFilterAsync(new List<ProductFilter>());

            // Assert   
            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
            Assert.Contains("Broken", result.Error);
        }


        [Fact]  
        public async Task GetProductsByFilterAsync_IfAllGucci_ReturnsAllGucci()
        {
            // Arrange  
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Variants = new List<ProductVariant>{new ProductVariant{Id = 1}}
                }
            };
            var expectedResult = Result<IEnumerable<Product>>.Ok(products);

            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllByFilterAsync(It.IsAny<IEnumerable<ProductFilter>>()))
                .ReturnsAsync(products);

            // Act      
            var result = await _productsService.GetProductsByFilterAsync(new List<ProductFilter>());


            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.NotEmpty(result.Value);
        }









        [Fact]
        public async Task CreateProductAsync_WhenCalled_ProcessesImagesAndAddsProductInUnitOfWork()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Products.Add(It.IsAny<Product>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            _mockedImagesService.Setup(s => s.GetProcessedImageWithExtension(It.IsAny<Image>())).Verifiable();

            var product = _productTestData.GetProduct(1);

            // Act      
            await _productsService.CreateProductAsync(product);

            // Assert   
            _mockedImagesService.Verify(x => x.GetProcessedImageWithExtension(It.IsAny<Image>()), Times.Once);
            _mockedUnitOfWork.Verify(uow => uow.Products.Add(product), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WhenCalled_CompletesTransaction()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Products.Add(It.IsAny<Product>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            var product = _productTestData.GetProduct(1);

            // Act      
            await _productsService.CreateProductAsync(product); 

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }


        [Fact]
        public async Task CreateProductAsync_IfCompletionFails_ReturnsFailedResult()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Products.Add(It.IsAny<Product>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Fail(""));
            var product = _productTestData.GetProduct(1);

            var expectedResult = Result<Product>.Fail("");

            // Act      
            var result = await _productsService.CreateProductAsync(product);

            // Assert   
            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
        }


        [Fact]
        public async Task CreateProductAsync_IfCompletionIsSuccessful_ReturnsOkResultWithObject()
        {
            // Arrange  
            var product = _productTestData.GetProduct(1);

            _mockedUnitOfWork.Setup(uow => uow.Products.Add(It.IsAny<Product>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAsync(It.IsAny<int>())).ReturnsAsync(product);
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            var expectedResult = Result<Product>.Ok(product);

            // Act      
            var result = await _productsService.CreateProductAsync(product);

            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.Equal(expectedResult.Value.Id, result.Value.Id);
        }












            

        [Fact]
        public async Task UpdateProductAsync_WhenCalled_ProcessesNewImageAndRemovesOldProductVariantsAndProductImages()
        {   
            // Arrange  
            var product = _productTestData.GetProduct(1);   
            var productForCreateOrUpdateDto = _productTestData.GetProductForCreateOrUpdateDto();

            _mockedUnitOfWork.Setup(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            _mockedImagesService.Setup(s => s.GetProcessedImageWithExtension(It.IsAny<Image>())).Verifiable();
                

            // Act          
            await _productsService.UpdateProductAsync(product, productForCreateOrUpdateDto);

            // Assert   
            _mockedImagesService.Verify(x => x.GetProcessedImageWithExtension(It.IsAny<Image>()), Times.Once);
            _mockedUnitOfWork.Verify(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>()), Times.Once);
        }


        [Fact]
        public async Task UpdateProductAsync_IfProductIsSuccessfullyUpdated_ReturnsOkResult()
        {       
            // Arrange      
            var product = _productTestData.GetProduct(1);
            var productForCreateOrUpdateDto = _productTestData.GetProductForCreateOrUpdateDto();

            _mockedUnitOfWork.Setup(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Images.Remove(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            var expectedResult = Result.Ok();

            // Act          
            var result = await _productsService.UpdateProductAsync(product, productForCreateOrUpdateDto);
            
            // Assert   

            Assert.Equal(expectedResult.Success, result.Success);
        }


        [Fact]
        public async Task UpdateProductAsync_IfUpdateFails_ReturnsFailedResult()
        {   
            // Arrange      
            var product = _productTestData.GetProduct(1);
            var productForCreateOrUpdateDto = _productTestData.GetProductForCreateOrUpdateDto();

            _mockedUnitOfWork.Setup(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Images.Remove(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Fail(""));

            var expectedResult = Result.Fail("");

            // Act          
            var result = await _productsService.UpdateProductAsync(product, productForCreateOrUpdateDto);

            // Assert   

            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
        }




            






            

        [Fact]
        public async Task DeleteProductAsync_WhenCalled_RemovesProductVariantsAndProductImagesAndProduct()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Products.Remove(It.IsAny<Product>())).Verifiable();

            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            var product = _productTestData.GetProduct(1);

            // Act          
            await _productsService.DeleteProductAsync(product);

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>()), Times.Once);
            _mockedUnitOfWork.Verify(uow => uow.Products.Remove(It.IsAny<Product>()), Times.Once);
        }


        [Fact]
        public async Task DeleteProductAsync_IfDeleteIsSuccessful_ReturnsOkResult()
        {   
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Images.Remove(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Products.Remove(It.IsAny<Product>())).Verifiable();

            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            var product = _productTestData.GetProduct(1);
            var expectedResult = Result.Ok();


            // Act          
            var result = await _productsService.DeleteProductAsync(product);

            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
        }


        [Fact]  
        public async Task DeleteProductAsync_IfDeleteFails_ReturnsFailedResult()
        {
            // Arrange  

            _mockedUnitOfWork.Setup(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Images.Remove(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Products.Remove(It.IsAny<Product>())).Verifiable();

            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Fail(""));

            var product = _productTestData.GetProduct(1);
            var expectedResult = Result.Fail("");


            // Act          
            var result = await _productsService.DeleteProductAsync(product);

            // Assert   

            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
        }












        [Fact]
        public async Task DeleteAllProductsInCategoryAsync_WhenCalled_GetsProductsInCategoryAndDeletesThemWithTheirDependencies()
        {
            // Arrange  
            var products = _productTestData.GetProducts();
            _mockedUnitOfWork.Setup(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Images.Remove(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Products.Remove(It.IsAny<Product>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.Products.GetAllProductsByCategoryAsync(It.IsAny<int>()))
                .ReturnsAsync(products);

            // Act          
             await _productsService.DeleteAllProductsInCategoryAsync(1);
                
            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Products.GetAllProductsByCategoryAsync(1), Times.Once);
            _mockedUnitOfWork.Verify(uow => uow.Variants.RemoveRange(It.IsAny<IEnumerable<ProductVariant>>()), Times.Exactly(products.Count()));
            _mockedImagesService.Verify(img => img.DeleteAsync(It.IsAny<int>()), Times.Exactly(products.Count()));
            _mockedUnitOfWork.Verify(uow => uow.Products.Remove(It.IsAny<Product>()), Times.Exactly(products.Count()));
        }
    }
}
        