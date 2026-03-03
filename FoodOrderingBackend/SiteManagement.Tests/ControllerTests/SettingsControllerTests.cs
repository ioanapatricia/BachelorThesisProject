using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SiteManagement.API.Controllers;
using SiteManagement.API.Entities;
using SiteManagement.API.Helpers;
using SiteManagement.API.Services.Interfaces;
using SiteManagement.Contracts.Dtos;
using Xunit;

namespace SiteManagement.Tests.ControllerTests
{
    public class SettingsControllerTests
    {
        private readonly Mock<ISettingsService> _mockedSettingsService;
        private readonly SettingsController _settingsController;
        private readonly IMapper _mapper;

        public SettingsControllerTests()
        {
            _mockedSettingsService = new Mock<ISettingsService>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _settingsController = new SettingsController(_mockedSettingsService.Object, _mapper);
        }

        [Fact]
        public async Task GetSettings_WhenCalled_CallsSettingsService()
        {
            // Act
            await _settingsController.GetSettings();

            // Assert
            _mockedSettingsService.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSettings_IfThereAreNoSettings_ReturnsNoContent()
        {
            // Arrange
            _mockedSettingsService.Setup(s => s.GetAsync()).ReturnsAsync((Func<SiteSettings>) null);

            // Act 

            var actualResult = await _settingsController.GetSettings();

            // Assert
            actualResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetSettings_IfSettingsExist_ReturnsOkWithSettings()
        {
            // Arrange
            var siteSettings = new SiteSettings {Id = "6072dc71ae4a10b6f7bd50b7"};
            _mockedSettingsService.Setup(s => s.GetAsync()).ReturnsAsync(siteSettings);

            // Act
            var actualResult = await _settingsController.GetSettings();

            //Assert
            actualResult.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult) actualResult;
            var responseBody = response.Value as SiteSettingsForReturnDto;
            Assert.NotNull(responseBody);
            Assert.Equal(responseBody.Id, siteSettings.Id);

        }








        [Fact]
        public async Task Update_WhenCalled_CallsGetInService()
        {
            // Act
            await _settingsController.Update(new SiteSettingsForUpdateDto());

            // Assert
            _mockedSettingsService.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_IfThereAreNoSettings_ReturnsNotFound()
        {
            // Arrange
            _mockedSettingsService.Setup(s => s.GetAsync()).ReturnsAsync((Func<SiteSettings>)null);

            // Act 

            var actualResult = await _settingsController.Update(new SiteSettingsForUpdateDto());

            // Assert
            actualResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task Update_IfUpdateFailsInService_ReturnsInternalServerError()
        {
            // Arrange
            _mockedSettingsService.Setup(s => s.GetAsync()).ReturnsAsync(new SiteSettings());
            _mockedSettingsService
                .Setup(settingsService =>
                    settingsService.UpdateAsync(It.IsAny<string>(), It.IsAny<SiteSettings>()))
                .ReturnsAsync(Result.Fail("error"));
            
            // Act
            var result = await _settingsController.Update(new SiteSettingsForUpdateDto());

            // Assert
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }

        [Fact]
        public async Task Update_IfUpdateIsSuccessfully_ReturnsNoContent()
        {
            // Arrange
            _mockedSettingsService.Setup(s => s.GetAsync()).ReturnsAsync(new SiteSettings());
            _mockedSettingsService
                .Setup(settingsService =>
                    settingsService.UpdateAsync(It.IsAny<string>(), It.IsAny<SiteSettings>()))
                .ReturnsAsync(Result.Ok);

            // Act
            var result = await _settingsController.Update(new SiteSettingsForUpdateDto());

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}
