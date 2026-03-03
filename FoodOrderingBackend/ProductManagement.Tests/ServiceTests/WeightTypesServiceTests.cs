using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services;
using ProductManagement.API.Services.Interfaces;
using Xunit;

namespace ProductManagement.Tests.ServiceTests
{
    public class WeightTypesServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
        private readonly IWeightTypesService _weightTypesService;

        public WeightTypesServiceTests()
        {
            _mockedUnitOfWork = new Mock<IUnitOfWork>();
            _weightTypesService = new WeightTypesService(_mockedUnitOfWork.Object);
        }

        [Fact]
        public async Task GetWeightTypesAsync_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.WeightTypes.GetAllAsync()).ReturnsAsync(new List<WeightType>());

            // Act
            await _weightTypesService.GetWeightTypesAsync();

            // Assert
            _mockedUnitOfWork.Verify(uow => uow.WeightTypes.GetAllAsync(), Times.Once);
        }
    }
}
