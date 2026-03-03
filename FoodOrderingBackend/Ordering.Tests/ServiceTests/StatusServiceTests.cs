using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Ordering.API.Entities;
using Ordering.API.Persistence.Interfaces;
using Ordering.API.Services;
using Ordering.API.Services.Interfaces;
using Xunit;

namespace Ordering.Tests.ServiceTests
{
    public class StatusServiceTests
    {
        private readonly Mock<IStatusRepository> _mockedStatusRepository;
        private readonly IStatusService _statusService;

        public StatusServiceTests()
        {
            _mockedStatusRepository = new Mock<IStatusRepository>();

            _statusService = new StatusService(_mockedStatusRepository.Object);
        }


        [Fact]
        public async Task GetAllAsync_WhenCalled_CallsOrdersRepository()
        {
            // Arrange
            _mockedStatusRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Status>());

            // Act      
            await _statusService.GetAllAsync();

            // Assert
            _mockedStatusRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }
    }
}
