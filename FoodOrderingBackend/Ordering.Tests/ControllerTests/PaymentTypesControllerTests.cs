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
    public class PaymentTypesControllerTests
    {
        private readonly Mock<IPaymentTypesService> _mockedPaymentsTypeService;
        private readonly PaymentTypesController _paymentTypesController;
        private readonly IMapper _mapper;
        public PaymentTypesControllerTests()
        {
            _mockedPaymentsTypeService = new Mock<IPaymentTypesService>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _paymentTypesController = new PaymentTypesController(_mockedPaymentsTypeService.Object, _mapper);
        }

        [Fact]
        public async Task GetPaymentTypes_WhenCalled_CallsPaymentTypesService()
        {
            // Act      
            await _paymentTypesController.GetAll();

            // Assert
            _mockedPaymentsTypeService.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPaymentTypes_IfNoPaymentTypeExists_ReturnsNoContent()
        {
            // Arrange
            _mockedPaymentsTypeService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<PaymentType>());

            // Act  
            var actualResult = await _paymentTypesController.GetAll();

            // Assert
            actualResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task GetPaymentTypes_IfThereIsOneOrMorePaymentTypes_ReturnsOkWithPaymentTypeList()
        {
            // Arrange  
            var paymentTypes = new List<PaymentType> { new() { Id = "60548f8c057aa97d88badb39" }, new() { Id = "60548f8c057aa97d88badb39" } };


            _mockedPaymentsTypeService.Setup(s => s.GetAllAsync()).ReturnsAsync(paymentTypes);

            // Act  
            var result = await _paymentTypesController.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as IEnumerable<PaymentTypeForGetDto>;
            Assert.NotNull(responseBody);


            Assert.All(responseBody, responseProductForGet =>
                Assert.Equal(paymentTypes.First(p => p.Id == responseProductForGet.Id).Id,
                    responseProductForGet.Id));
        }
    }
}
