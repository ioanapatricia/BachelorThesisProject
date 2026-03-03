using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Global.Contracts;
using ProductManagement.API.Controllers;
using ProductManagement.API.Entities;
using ProductManagement.API.Helpers.MapperProfiles;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Tests.TestData;
using Xunit;

namespace ProductManagement.Tests.ControllerTests
{
    public class ImagesControllerTests
    {
        private readonly Mock<IImagesService> _mockedImagesService;
        private readonly Mock<IImageValidator> _mockedImageValidator;
        private readonly ImagesController _imagesController;
        private readonly IMapper _mapper;
        private readonly ImageTestData _imageTestData;

        public ImagesControllerTests()
        {
            _mockedImagesService = new Mock<IImagesService>();
            _mockedImageValidator = new Mock<IImageValidator>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MapperProfiles()));
            _mapper = new Mapper(configuration);

            _imagesController = new ImagesController(_mockedImagesService.Object, _mockedImageValidator.Object, _mapper);

            _imageTestData = new ImageTestData();
        }

        [Fact]
        public async Task CreateImage_WhenCalled_CallsValidateImage()
        {
            // Arrange
            _mockedImageValidator.Setup(s => s.ValidateImage(It.IsAny<ImageForCreateDto>())).Returns(Result.Ok());

            _mockedImagesService.Setup(s => s.CreateImageAsync(It.IsAny<Image>()))
                .ReturnsAsync(Result<Image>.Ok(new Image()));

            // Act  

            await _imagesController.CreateImage(_imageTestData.GetImageForCreateDto());

            //Assert
            _mockedImageValidator.Verify(s => s.ValidateImage(It.IsAny<ImageForCreateDto>()), Times.Once);
        }

        [Fact]
        public async Task CreateImage_IfValidationFails_ReturnsBadRequest()
        {
            // Arrange
            _mockedImageValidator.Setup(s => s.ValidateImage(It.IsAny<ImageForCreateDto>())).Returns(Result.Fail(""));

            // Act  
            var result = await _imagesController.CreateImage(new ImageForCreateDto());

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Fact]
        public async Task CreateImage_IfValidationIsGucci_CallsCreateInService()
        {
            // Arrange
            _mockedImageValidator.Setup(s => s.ValidateImage(It.IsAny<ImageForCreateDto>())).Returns(Result.Ok());
            _mockedImagesService.Setup(s => s.CreateImageAsync(It.IsAny<Image>())).ReturnsAsync(Result<Image>.Ok(new Image()));

            // Act  
            await _imagesController.CreateImage(_imageTestData.GetImageForCreateDto());

            //Assert
            _mockedImagesService.Verify(s => s.CreateImageAsync(It.IsAny<Image>()), Times.Once);
        }

        [Fact]
        public async Task CreateImage_IfImageCreationFails_ReturnsInternalServerError()
        {
            // Arrange
            _mockedImageValidator.Setup(s => s.ValidateImage(It.IsAny<ImageForCreateDto>())).Returns(Result.Ok());
            _mockedImagesService.Setup(s => s.CreateImageAsync(It.IsAny<Image>())).ReturnsAsync(Result<Image>.Fail(""));

            // Act  
            var result = await _imagesController.CreateImage(_imageTestData.GetImageForCreateDto());

            // Assert
            result.Should().BeOfType<ObjectResult>();

            var errorResult = result as ObjectResult;

            Assert.Equal(500, errorResult.StatusCode);
        }

            
        [Fact]  
        public async Task CreateImage_IfImageCreationIsSuccessful_ReturnsCreatedAtRoute()
        {
            // Arrange
            _mockedImageValidator.Setup(s => s.ValidateImage(It.IsAny<ImageForCreateDto>())).Returns(Result.Ok());

            var expectedImageResult = new Image {Id = 1};
            _mockedImagesService.Setup(s => s.CreateImageAsync(It.IsAny<Image>())).ReturnsAsync(Result<Image>.Ok(expectedImageResult));


            // Act  
            var result = await _imagesController.CreateImage(_imageTestData.GetImageForCreateDto());

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();

            var response = (CreatedAtRouteResult)result;
            var responseBody = response.Value as ImageForGetDto;

            Assert.NotNull(responseBody);
            Assert.Equal(responseBody.Id, expectedImageResult.Id);
        }



        [Fact]
        public async Task GetImageForDisplay_WhenCalled_CallsGetImageInService()
        {
            // Arrange
            _mockedImagesService.Setup(s => s.GetImageAsync(It.IsAny<int>())).ReturnsAsync(_imageTestData.GetImage());

            var imagesController = new ImagesController(_mockedImagesService.Object, _mockedImageValidator.Object, _mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act 
            await imagesController.GetImageForDisplay(1);


            // Assert
            _mockedImagesService.Verify(s => s.GetImageAsync(It.IsAny<int>()), Times.Once);
        }


        [Fact]
        public async Task GetImageForDisplay_IfImageIsNull_ReturnsNotFound()
        {
            // Arrange
            _mockedImagesService.Setup(s => s.GetImageAsync(It.IsAny<int>())).ReturnsAsync((Func<Image>)null);

            var imagesController = new ImagesController(_mockedImagesService.Object, _mockedImageValidator.Object, _mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act 
            var result = await imagesController.GetImageForDisplay(1);


            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

            
        [Fact]
        public async Task GetImageForDisplay_IfImageIsFound_ReturnsFileStreamResult()
        {
            // Arrange
            _mockedImagesService.Setup(s => s.GetImageAsync(It.IsAny<int>())).ReturnsAsync(_imageTestData.GetImage());

            var imagesController = new ImagesController(_mockedImagesService.Object, _mockedImageValidator.Object, _mapper)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };  

            // Act 
            var result = await imagesController.GetImageForDisplay(1);


            // Assert
            result.Should().BeOfType<FileStreamResult>();
        }

        [Fact]
        public async Task GetImageAsBase64String_WhenCalled_CallsGetImageAsBase64StringInService()
        {
            // Arrange 
            _mockedImagesService.Setup(s => s.GetImageAsBase64String(It.IsAny<int>())).ReturnsAsync(_imageTestData.GetImageAsBase64String());

            // Act 
            await _imagesController.GetImageAsBase64String(1);

            // Assert

            _mockedImagesService.Verify(s => s.GetImageAsBase64String(It.IsAny<int>()), Times.Once);

        }

        [Fact]
        public async Task GetImageAsBase64String_IfImageIsNull_ReturnsNotFound()
        {
            // Arrange
            _mockedImagesService.Setup(s => s.GetImageAsBase64String(It.IsAny<int>()))
                .ReturnsAsync((Func<string>) null);


            // Act 
            var result = await _imagesController.GetImageAsBase64String(1);


            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task GetImageAsBase64String_IfTheImageExists_ReturnsOkImage()
        {
            // Arrange  
            var image = _imageTestData.GetImageAsBase64String();

            _mockedImagesService.Setup(s => s.GetImageAsBase64String(It.IsAny<int>())).ReturnsAsync(image);

            // Act  
            var result = await _imagesController.GetImageAsBase64String(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var response = (OkObjectResult)result;
            var responseBody = response.Value as string;
            Assert.NotNull(responseBody);
            Assert.Equal(responseBody, image);
        }
    }
}
