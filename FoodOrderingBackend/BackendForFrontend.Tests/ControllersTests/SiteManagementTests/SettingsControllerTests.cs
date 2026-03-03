using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BackendForFrontend.API.Controllers.SiteManagement;
using BackendForFrontend.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using SiteManagement.Contracts.Dtos;
using Xunit;

namespace BackendForFrontend.Tests.ControllersTests.SiteManagementTests
{
    public class SettingsControllerTests
    {
        private readonly Mock<IControllerHelper> _mockedControllerHelper;
        public SettingsControllerTests()
        {
            _mockedControllerHelper = new Mock<IControllerHelper>();
        }


        [Fact]
        public async Task GetSettings_WhenCalled_CallsSmApi()
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

            var settingsController = new SettingsController(mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await settingsController.GetSettings();

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }



        [Fact]
        public async Task Update_WhenCalled_CallsSmApi()
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

            var settingsController = new SettingsController(mockedClientFactory.Object, _mockedControllerHelper.Object);


            // Act  
            await settingsController.Update(new SiteSettingsForUpdateDto());

            // Assert

            mockMessageHandler.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
            _mockedControllerHelper.Verify(_ => _.ParseActionResult(It.IsAny<HttpResponseMessage>(), It.IsAny<string>()), Times.Once());
        }
    }
}
