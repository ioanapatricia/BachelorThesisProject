using System.Threading.Tasks;
using Global.Contracts;
using Moq;
using ProductManagement.API.Entities;
using ProductManagement.API.Persistence;
using ProductManagement.API.Services;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.Tests.TestData;
using Xunit;

namespace ProductManagement.Tests.ServiceTests
{
    public class ImagesServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockedUnitOfWork;
        private readonly IImagesService _imagesService;
        private readonly ImageTestData _imageTestData;
        public ImagesServiceTests()
        {
            _mockedUnitOfWork = new Mock<IUnitOfWork>();

            _imagesService = new ImagesService(_mockedUnitOfWork.Object);

            _imageTestData = new ImageTestData();
        }



        [Fact]
        public async Task GetImageAsync_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            _mockedUnitOfWork.Setup(uow => uow.Images.GetAsync(It.IsAny<int>())).ReturnsAsync(new Image());

            // Act  
            await _imagesService.GetImageAsync(1);

            // Assert
            _mockedUnitOfWork.Verify(uow => uow.Images.GetAsync(1), Times.Once);
        }


        [Fact]
        public async Task CreateImageAsync_WhenCalled_AddsImageInUnitOfWork()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Images.Add(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);

            var image = new Image { Name = "name.png"};
            // Act      
            await _imagesService.CreateImageAsync(image);

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.Images.Add(It.IsAny<Image>()), Times.Once);
        }

        [Fact]
        public async Task CreateImageAsync_WhenCalled_CompletesTransaction()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Images.Add(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            var image = new Image { Name = "name.png" };

            // Act      
            await _imagesService.CreateImageAsync(image);

            // Assert   
            _mockedUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }


        [Fact]  
        public async Task CreateImageAsync_IfCreationIsFailed_ReturnsFailedResult()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Images.Add(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Fail(""));
            var image = new Image { Name = "name.png" };

            var expectedResult = Result<Image>.Fail("");

            // Act      
            var result = await _imagesService.CreateImageAsync(image);

            // Assert   
            Assert.Equal(expectedResult.IsFailure, result.IsFailure);
        }
            

        [Fact]
        public async Task CreateImageAsync_IfCreationIsSuccessful_ReturnsOkResult()
        {
            // Arrange  
            _mockedUnitOfWork.Setup(uow => uow.Images.Add(It.IsAny<Image>())).Verifiable();
            _mockedUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(Result.Ok);
            var image = new Image { Name = "name.png" };

            var expectedResult = Result<Image>.Ok(new Image());

            // Act      
            var result = await _imagesService.CreateImageAsync(image);

            // Assert   
            Assert.Equal(expectedResult.Success, result.Success);
        }

        [Fact]
        public async Task GetImageAsBase64String_WhenCalled_CallsUnitOfWork()
        {
            // Arrange
            var image = _imageTestData.GetImage();
            _mockedUnitOfWork.Setup(uow => uow.Images.GetAsync(It.IsAny<int>())).ReturnsAsync(image);

            // Act  
            await _imagesService.GetImageAsBase64String(1);

            // Assert
            _mockedUnitOfWork.Verify(uow => uow.Images.GetAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetImageAsBase64String_WhenCalled_ReturnsBase64String()
        {
            // Arrange
            var image = _imageTestData.GetImage();
            _mockedUnitOfWork.Setup(uow => uow.Images.GetAsync(It.IsAny<int>())).ReturnsAsync(image);

            // Act  
            var result = await _imagesService.GetImageAsBase64String(1);

            // Assert
            Assert.NotEmpty(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetProcessedImageWithExtension_WhenCalled_ReturnsProcessedImage()
        {
            // Arrange
            Image image = new()
            {
                Id = 1,
                Data = _imageTestData.GetImageAsByteArray(),
                Name = "test.jpg"
            };

            // Act  
            var result = _imagesService.GetProcessedImageWithExtension(image);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(image.Id, result.Id);
            Assert.Equal("test", result.Name);
            Assert.Equal("jpg", result.Extension);
            Assert.Equal(image.Data, result.Data);
        }
    }
}
    