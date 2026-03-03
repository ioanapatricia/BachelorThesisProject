using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Contracts;
using Moq;
using Ordering.API.Entities;
using Ordering.API.Persistence.Interfaces;
using Ordering.API.Services;
using Ordering.API.Services.Interfaces;
using Xunit;

namespace Ordering.Tests.ServiceTests
{
    public class OrdersServiceTests
    {
        private readonly Mock<IOrdersRepository> _mockedOrdersRepository;
        private readonly Mock<IStatusRepository> _mockedStatusRepository;
        private readonly IOrdersService _ordersService;
        public OrdersServiceTests()
        {
            _mockedOrdersRepository = new Mock<IOrdersRepository>();
            _mockedStatusRepository = new Mock<IStatusRepository>();

            _ordersService = new OrdersService(_mockedOrdersRepository.Object, _mockedStatusRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenCalled_CallsOrdersRepository()
        {
            // Arrange
            _mockedOrdersRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Order>());

            // Act      
            await _ordersService.GetAllAsync();

            // Assert
            _mockedOrdersRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }


        [Fact]  
        public async Task GetAsync_WhenCalled_CallsOrdersRepository()
        {
            // Arrange
            _mockedOrdersRepository.Setup(r => r.GetAsync(It.IsAny<string>())).ReturnsAsync(new Order());

            // Act  
            await _ordersService.GetAsync("testId");

            // Assert
            _mockedOrdersRepository.Verify(r => r.GetAsync("testId"), Times.Once);
        }







        [Fact]
        public async Task CreateAsync_WhenCalled_CallsStatusRepositoryToGetInitialStatus()
        {
            // Arrange
            _mockedOrdersRepository.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync(Result<Order>.Ok(new Order()));
            _mockedStatusRepository.Setup(r => r.GetByName(It.IsAny<string>())).ReturnsAsync(new Status());
            var order = new Order { Products = new List<Product> { new() { Price = 1 }, new() { Price = 1 } } };

            // Act  
            await _ordersService.CreateAsync(order);

            // Assert
            _mockedStatusRepository.Verify(r => r.GetByName(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_CallsCreateInOrdersRepository()
        {
            // Arrange
            _mockedOrdersRepository.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync(Result<Order>.Ok(new Order()));
            _mockedStatusRepository.Setup(r => r.GetByName(It.IsAny<string>())).ReturnsAsync(new Status());
            var order = new Order { Products = new List<Product> { new() { Price = 1 }, new() { Price = 1 } } };

            // Act  
            await _ordersService.CreateAsync(order);    

            // Assert
            _mockedOrdersRepository.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Once);
        }



        [Fact]
        public async Task CreateAsync_IfInitialStatusIsNull_ReturnsFailedResult()
        {
            // Arrange
            _mockedOrdersRepository.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync(Result<Order>.Ok(new Order()));
            _mockedStatusRepository.Setup(r => r.GetByName(It.IsAny<string>())).ReturnsAsync((Func<Status>)null);
            var order = new Order { Products = new List<Product> { new() { Price = 1 }, new() { Price = 1 } } };
            var expectedResult = Result<Order>.Fail("");

            // Act  
            var result =  await _ordersService.CreateAsync(order);

            // Assert
            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
        }


        [Fact]
        public async Task CreateAsync_IfInitialStatusIsNull_DoesNotCallCreateInRepository()
        {
            // Arrange
            _mockedOrdersRepository.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync(Result<Order>.Ok(new Order()));
            _mockedStatusRepository.Setup(r => r.GetByName(It.IsAny<string>())).ReturnsAsync((Func<Status>)null);
            var order = new Order { Products = new List<Product> { new() { Price = 1 }, new() { Price = 1 } } };

            // Act      
            await _ordersService.CreateAsync(order);

            // Assert
            _mockedOrdersRepository.Verify(r => r.CreateAsync(It.IsAny<Order>()), Times.Never);
        }

            
        [Fact]
        public async Task CreateAsync_IfStatusExistsAndCreationIsSuccessfulInRepository_ReturnsOkResultWithOrder()
        {
            // Arrange
            var order = new Order { Id = "testId", Products = new List<Product> { new() { Price = 1 }, new() { Price = 1 } } };

            _mockedOrdersRepository.Setup(r => r.CreateAsync(It.IsAny<Order>())).ReturnsAsync(Result<Order>.Ok(order));
            _mockedStatusRepository.Setup(r => r.GetByName(It.IsAny<string>())).ReturnsAsync(new Status());
            var expectedResult = Result<Order>.Ok(order);

            // Act      
            var result = await _ordersService.CreateAsync(order);

            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
            Assert.Equal(expectedResult.Value.Id, result.Value.Id);
        }
    }   
}
