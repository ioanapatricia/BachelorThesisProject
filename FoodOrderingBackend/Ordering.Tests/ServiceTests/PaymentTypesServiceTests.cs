using System;
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
    public class PaymentTypesServiceTests
    {
        private readonly Mock<IPaymentTypesRepository> _mockedPaymentTypesRepository;
        private readonly IPaymentTypesService _paymentTypesService;
        public PaymentTypesServiceTests()
        {
            _mockedPaymentTypesRepository = new Mock<IPaymentTypesRepository>();

            _paymentTypesService = new PaymentTypesService(_mockedPaymentTypesRepository.Object);
        }


        [Fact]
        public async Task GetAllAsync_WhenCalled_CallsPaymentTypesRepository()
        {
            // Arrange
            _mockedPaymentTypesRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<PaymentType>());

            // Act      
            await _paymentTypesService.GetAllAsync();

            // Assert
            _mockedPaymentTypesRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }


        [Fact]
        public async Task ExistsAsync_WhenCalled_CallsGetInPaymentTypesRepository()
        {
            // Arrange
            _mockedPaymentTypesRepository.Setup(r => r.Get(It.IsAny<string>())).ReturnsAsync(new PaymentType());

            // Act
            await _paymentTypesService.ExistsAsync("testId");

            // Assert
            _mockedPaymentTypesRepository.Verify(r => r.Get("testId"), Times.Once);
        }


        [Fact]
        public async Task ExistsAsync_IfPaymentTypeExists_ReturnsTrue()
        {
            // Arrange
            _mockedPaymentTypesRepository.Setup(r => r.Get(It.IsAny<string>())).ReturnsAsync(new PaymentType());

            // Act  
            var result = await _paymentTypesService.ExistsAsync("testId");

            // Assert
            Assert.True(result);
        }


        [Fact]  
        public async Task ExistsAsync_IfPaymentTypeDoesNotExist_ReturnsFalse()
        {
            // Arrange
            _mockedPaymentTypesRepository.Setup(r => r.Get(It.IsAny<string>())).ReturnsAsync((Func<PaymentType>)null);

            // Act  
            var result = await _paymentTypesService.ExistsAsync("testId");

            // Assert
            Assert.False(result);
        }
    }
}
    