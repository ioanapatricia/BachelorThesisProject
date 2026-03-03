using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BackendForFrontend.API.Controllers.ProductManagement;
using BackendForFrontend.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ProductManagement.Contracts.Dtos;
using Xunit;

namespace BackendForFrontend.Tests.ControllersTests.ProductManagementTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IControllerHelper> _mockedControllerHelper;
        public ProductsControllerTests()
        {
            _mockedControllerHelper = new Mock<IControllerHelper>();
        }


        [Fact]  
        public async Task GetProduct_WhenCalled_CallsPmApi()
        {
            // Arrange
            var mockedClientFactory = new Mock<IHttpClientFactory>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                });

            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:6666/api/")
            };

            mockedClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);


            _mockedControllerHelper.Setup(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), null))
                .ReturnsAsync(new OkObjectResult(""));

            var productsController = new ProductsController(mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await productsController.GetProduct(1);

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }






        [Fact]
        public async Task GetProducts_WhenCalled_CallsPmApi()
        {
            // Arrange
            var mockedClientFactory = new Mock<IHttpClientFactory>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                });

            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:6666/api/")
            };

            mockedClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);


            _mockedControllerHelper.Setup(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), null))
                .ReturnsAsync(new OkObjectResult(""));

            var productsController = new ProductsController(mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await productsController.GetProducts();

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }




        [Fact]
        public async Task Create_WhenCalled_CallsPmApi()
        {
            // Arrange
            var mockedClientFactory = new Mock<IHttpClientFactory>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:6666/api/")
            };

            mockedClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);


            _mockedControllerHelper.Setup(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), null))
                .ReturnsAsync(new OkObjectResult(""));

            var productsController = new ProductsController(mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await productsController.Create(new ProductForCreateOrUpdateDto());

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }






        [Fact]
        public async Task Update_WhenCalled_CallsPmApi()
        {
            // Arrange
            var mockedClientFactory = new Mock<IHttpClientFactory>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage   
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:6666/api/")
            };

            mockedClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);


            _mockedControllerHelper.Setup(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), null))
                .ReturnsAsync(new OkObjectResult(""));

            var productsController = new ProductsController(mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await productsController.Update(1, new ProductForCreateOrUpdateDto());

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }





        [Fact]
        public async Task Delete_WhenCalled_CallsPmApi()
        {   
            // Arrange
            var mockedClientFactory = new Mock<IHttpClientFactory>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:6666/api/")
            };

            mockedClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);


            _mockedControllerHelper.Setup(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), null))
                .ReturnsAsync(new OkObjectResult(""));

            var productsController = new ProductsController(mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await productsController.Delete(1);

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }
    }
}
    