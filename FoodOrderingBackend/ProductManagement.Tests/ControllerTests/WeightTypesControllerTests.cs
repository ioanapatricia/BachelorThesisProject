using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagement.API.Controllers;
using ProductManagement.API.Entities;
using ProductManagement.API.Helpers.MapperProfiles;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Tests.TestData;
using Xunit;

namespace ProductManagement.Tests.ControllerTests
{
    public class WeightTypesControllerTests
    {
        private readonly Mock<IWeightTypesService> _mockedWeightTypesService;
        private readonly IMapper _mapper;
        private readonly WeightTypesController _weightTypesController;
        private readonly WeightTypeTestData _weightTypeTestData;
        public WeightTypesControllerTests()
        {
            _mockedWeightTypesService = new Mock<IWeightTypesService>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);
            _weightTypesController = new WeightTypesController(_mockedWeightTypesService.Object, _mapper);
            _weightTypeTestData = new WeightTypeTestData();
        }

        [Fact]
        public async Task GetWeightTypes_WhenCalled_CallsWeightTypesService()
        {
            // Act
            await _weightTypesController.GetWeightTypes();

            //Assert
            _mockedWeightTypesService.Verify(s => s.GetWeightTypesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetWeightTypes_IfThereIsNoWeightTypes_ReturnsNoContent()
        {
            // Arrange
            _mockedWeightTypesService.Setup(s => s.GetWeightTypesAsync()).ReturnsAsync(new List<WeightType>());

            // Act
            var result = await _weightTypesController.GetWeightTypes();

            // Assert
            result.Should().BeOfType<NoContentResult>();

        }

        [Fact]
        public async Task GetWeightTypes_IfThereIsOneOrMoreWeightTypes_ReturnsOkWithWeightTypesList()
        {
            // Arrange 
            var weightTypes = _weightTypeTestData.GetWeightTypes();

            _mockedWeightTypesService.Setup(s => s.GetWeightTypesAsync()).ReturnsAsync(weightTypes);

            // Act
            var result = await _weightTypesController.GetWeightTypes();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult) result;
            var responseBody = response.Value as IEnumerable<WeightTypeDto>;
            Assert.NotNull(responseBody);

            Assert.All(responseBody, weightTypeDto => Assert.Equal(weightTypes.First(wt => wt.Id == weightTypeDto.Id).Id, weightTypeDto.Id));
        }
    }
}
