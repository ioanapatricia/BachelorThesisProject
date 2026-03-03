using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ordering.API.Controllers;
using Ordering.API.Entities;
using Ordering.API.Helpers;
using Ordering.API.Services.Interfaces;
using Ordering.Contracts.Dtos;
using Xunit;

namespace Ordering.Tests.ControllerTests
{
    public class StatusControllerTests
    {
        private readonly Mock<IStatusService> _mockedStatusService;
        private readonly StatusController _statusController;
        private readonly IMapper _mapper;
        public StatusControllerTests()
        {
            _mockedStatusService = new Mock<IStatusService>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _statusController = new StatusController(_mockedStatusService.Object, _mapper);
        }

        [Fact]
        public async Task GetStatus_WhenCalled_CallsStatusService()
        {
            // Act      
            await _statusController.GetAll();

            // Assert
            _mockedStatusService.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetStatus_IfNoStatusExists_ReturnsNoContent()
        {
            // Arrange
            _mockedStatusService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Status>());

            // Act  
            var actualResult = await _statusController.GetAll();

            // Assert
            actualResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetStatus_IfThereIsOneOrMoreStatus_ReturnsOkWithStatusList()
        {
            // Arrange  
            var status = new List<Status> { new() { Id = "60548f8c057aa97d88badb39" }, new() { Id = "60548f8c057aa97d88badb39" } };


            _mockedStatusService.Setup(s => s.GetAllAsync()).ReturnsAsync(status);

            // Act  
            var result = await _statusController.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as IEnumerable<StatusForGetDto>;
            Assert.NotNull(responseBody);


            Assert.All(responseBody, responseProductForGet =>
                Assert.Equal(status.First(p => p.Id == responseProductForGet.Id).Id,
                    responseProductForGet.Id));
        }
    }
}
