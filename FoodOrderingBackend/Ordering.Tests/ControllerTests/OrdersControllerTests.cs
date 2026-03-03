using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Global.Contracts;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Controllers;
using Moq;
using Ordering.API.Entities;
using Ordering.API.Helpers;
using Ordering.API.Services.Interfaces;
using Ordering.Contracts.Dtos;
using Xunit;

namespace Ordering.Tests.ControllerTests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrdersService> _mockedOrdersService;
        private readonly Mock<IPaymentTypesService> _mockedPaymentsTypeService;
        private readonly OrdersController _ordersController;
        private readonly IMapper _mapper;
        public OrdersControllerTests()
        {
            _mockedOrdersService = new Mock<IOrdersService>();
            _mockedPaymentsTypeService = new Mock<IPaymentTypesService>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _ordersController = new OrdersController(_mockedOrdersService.Object, _mockedPaymentsTypeService.Object, _mapper);
        }

        [Fact]
        public async Task GetOrder_WhenCalled_CallsOrdersService()
        {
            // Act      
            await _ordersController.Get("60548f8c057aa97d88badb39");

            // Assert
            _mockedOrdersService.Verify(s => s.GetAsync("60548f8c057aa97d88badb39"), Times.Once);
        }


        [Fact]
        public async Task GetOrder_IfCalledWithInvalidHexString_ReturnsBadRequest()
        {
            // Act      
            var actualResult = await _ordersController.Get("invalidHexString");

            // Assert
            actualResult.Should().BeOfType<BadRequestObjectResult>();   
        }
            
        [Fact]
        public async Task GetOrder_IfOrderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockedOrdersService.Setup(s => s.GetAsync(It.IsAny<string>())).ReturnsAsync((Func<Order>)null);

            // Act      
            var actualResult = await _ordersController.Get("60548f8c057aa97d88badb39");

            // Assert
            actualResult.Should().BeOfType<NotFoundObjectResult>();
        }   


        [Fact]
        public async Task GetOrder_IfOrderExists_ReturnsOkWithOrder()
        {
            // Arrange
            var order = new Order {Id = "60548f8c057aa97d88badb39"};
            _mockedOrdersService.Setup(s => s.GetAsync(It.IsAny<string>())).ReturnsAsync(order);

            // Act      
            var actualResult = await _ordersController.Get("60548f8c057aa97d88badb39");

            // Assert
            actualResult.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)actualResult;
            var responseBody = response.Value as OrderForGetDto;
            Assert.NotNull(responseBody);
            Assert.Equal(responseBody.Id, order.Id);
        }










        [Fact]
        public async Task GetOrders_WhenCalled_CallsOrdersService()
        {
            // Act      
            await _ordersController.GetAll();

            // Assert
            _mockedOrdersService.Verify(s => s.GetAllAsync(), Times.Once);
        }



        [Fact]
        public async Task GetOrders_IfThereIsOneOrMoreOrders_ReturnsOkWithOrderList()
        {
            // Arrange  
            var orders = new List<Order> {new() {Id = "60548f8c057aa97d88badb39" }, new() { Id = "60548f8c057aa97d88badb39" } };


            _mockedOrdersService.Setup(s => s.GetAllAsync()).ReturnsAsync(orders);

            // Act  
            var result = await _ordersController.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as IEnumerable<OrderForGetDto>;
            Assert.NotNull(responseBody);


            Assert.All(responseBody, responseProductForGet =>
                Assert.Equal(orders.First(p => p.Id == responseProductForGet.Id).Id,
                    responseProductForGet.Id));
        }

        [Fact]
        public async Task GetOrders_IfNoOrderExists_ReturnsNoContent()
        {
            // Arrange
            _mockedOrdersService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Order>());

            // Act  
            var actualResult = await _ordersController.GetAll();

            // Assert
            actualResult.Should().BeOfType<NoContentResult>();
        }












            

        [Fact]
        public async Task Create_IfGivenPaymentTypeDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            _mockedPaymentsTypeService.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            // Act  
            var actualResult = await _ordersController.Create(new NormalizedOrderForCreationDto
                {PaymentTypeId = "60548f8c057aa97d88badb39" });

            // Assert
            actualResult.Should().BeOfType<BadRequestObjectResult>();
        }
            
        [Fact]
        public async Task Create_IfOrderIsValid_CallsCreateInService()
        {
            // Arrange
            var order = new NormalizedOrderForCreationDto
                {PaymentTypeId = "60548f8c057aa97d88badb39" };

            _mockedPaymentsTypeService.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _mockedOrdersService.Setup(s => s.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(Result<Order>.Ok(new Order()));

            // Act  
            await _ordersController.Create(order);

            // Assert
            _mockedOrdersService.Verify(s => s.CreateAsync(It.IsAny<Order>()), Times.Once);
        }


        [Fact]
        public async Task Create_IfCreationFailsInService_ReturnsInternalServerError()
        {
            // Arrange
            var order = new NormalizedOrderForCreationDto
                { PaymentTypeId = "60548f8c057aa97d88badb39" };

            _mockedPaymentsTypeService.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _mockedOrdersService.Setup(s => s.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(Result<Order>.Fail(""));

            // Act  
            var result = await _ordersController.Create(order);

            // Assert
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }


        [Fact]
        public async Task Create_IfOrderIsSuccessfullyCreated_ReturnsCreatedAtRoute()
        {
            // Arrange
            var order = new NormalizedOrderForCreationDto
                { PaymentTypeId = "60548f8c057aa97d88badb39" };

            _mockedPaymentsTypeService.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            _mockedOrdersService.Setup(s => s.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(Result<Order>.Ok(new Order()));

            // Act      
            var result = await _ordersController.Create(order);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();

            var response = (CreatedAtRouteResult)result;
            var responseBody = response.Value as OrderForGetDto;

            Assert.NotNull(responseBody);
        }
    }
}
