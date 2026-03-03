using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using ProductManagement.API.Entities;
using ProductManagement.API.Helpers;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.API.Validators.Interfaces;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService _imagesService;
        private readonly IImageValidator _imageValidator;
        private readonly IMapper _mapper;

        public ImagesController(IImagesService imagesService, IImageValidator imageValidator, IMapper mapper)
        {
            _imagesService = imagesService;
            _imageValidator = imageValidator;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImage(ImageForCreateDto imageForCreateDto)
        {
            var validationResult = _imageValidator.ValidateImage(imageForCreateDto);
            if (validationResult.IsFailure)
                return BadRequest(validationResult.Error);

            var image = _mapper.Map<Image>(imageForCreateDto);

            var result = await _imagesService.CreateImageAsync(image);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Could not create this image {result.Error}");

            var imageForReturn = _mapper.Map<ImageForGetDto>(result.Value);

            return CreatedAtRoute("GetImageForDisplay", new {id = result.Value.Id}, imageForReturn);
        }

        [HttpGet("{id}", Name = "GetImageForDisplay")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(File))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageForDisplay(int id)
        {
            var image = await _imagesService.GetImageAsync(id);
            if (image is null)
            {
                return NotFound($"There is no image with ID {id} in the database.");
            }

            var mimeType = image.Extension.GetMimeTypeFromString();

            var stream = new MemoryStream(image.Data);

            var cd = new System.Net.Mime.ContentDisposition
            {
                Inline = true
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());

            return File(stream, mimeType);
        }

        [HttpGet("asBase64/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageAsBase64String(int id)
        {
            var image = await _imagesService.GetImageAsBase64String(id);

            if (image is null)
                return NotFound($"There is no image with ID {id} in the database.");

            return Ok(image);
        }
    }
}
