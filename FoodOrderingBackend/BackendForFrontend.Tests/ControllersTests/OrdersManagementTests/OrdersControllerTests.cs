using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BackendForFrontend.API.Controllers.OrdersManagement;
using BackendForFrontend.API.Controllers.ProductManagement;
using BackendForFrontend.API.Helpers;
using BackendForFrontend.API.Services.BackendForFrontend;
using BackendForFrontend.API.Services.OrdersManagement;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Xunit;

namespace BackendForFrontend.Tests.ControllersTests.OrdersManagementTests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IControllerHelper> _mockedControllerHelper;
        private readonly Mock<IOrdersService> _mockedOrdersService;
        private readonly Mock<IUsersService> _mockedUsersService;

        public OrdersControllerTests()
        {
            _mockedControllerHelper = new Mock<IControllerHelper>();
            _mockedOrdersService = new Mock<IOrdersService>();
            _mockedUsersService = new Mock<IUsersService>();
        }


        [Fact]
        public async Task GetOrder_WhenCalled_CallsOrderingApi()
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

            var ordersController = new OrdersController(_mockedOrdersService.Object, _mockedUsersService.Object, mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await ordersController.GetOrder("6070563c815ceb6f8aac1152");

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetOrders_WhenCalled_CallsOrderingApi()
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

            var ordersController = new OrdersController(_mockedOrdersService.Object, _mockedUsersService.Object, mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await ordersController.GetOrders();

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }
    }
}
