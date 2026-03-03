using System.Threading.Tasks;
using Global.Contracts;
using Moq;
using SiteManagement.API.Entities;
using SiteManagement.API.Persistence.Interfaces;
using SiteManagement.API.Services;
using SiteManagement.API.Services.Interfaces;
using Xunit;

namespace SiteManagement.Tests.ServiceTests
{
    public class SettingsServiceTests
    {
        private readonly Mock<ISettingsRepository> _mockedSettingsRepository;
        private readonly ISettingsService _settingsService;
        public SettingsServiceTests()
        {
            _mockedSettingsRepository = new Mock<ISettingsRepository>();

            _settingsService = new SettingsService(_mockedSettingsRepository.Object);
        }

        [Fact]
        public async Task GetAsync_WhenCalled_CallsSettingsRepository()
        {
            // Arrange
            _mockedSettingsRepository.Setup(s => s.GetAsync()).ReturnsAsync(new SiteSettings());

            // Act      
            await _settingsService.GetAsync();

            // Assert
            _mockedSettingsRepository.Verify(s => s.GetAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_WhenCalled_CallsSettingsRepository()
        {
            // Arrange
            _mockedSettingsRepository
                .Setup(settingsService =>
                    settingsService.UpdateAsync(It.IsAny<string>(), It.IsAny<SiteSettings>()))
                .ReturnsAsync(Result.Ok);

            SiteSettings siteSettings = new();

            // Act      
            await _settingsService.UpdateAsync("6072dc71ae4a10b6f7bd50b7", siteSettings);

            // Assert
            _mockedSettingsRepository.Verify(s => s.UpdateAsync("6072dc71ae4a10b6f7bd50b7", siteSettings), Times.Once); 
        }
    }
}
