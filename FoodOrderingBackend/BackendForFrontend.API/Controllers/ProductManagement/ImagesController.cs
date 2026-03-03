using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Contracts.Dtos;

namespace BackendForFrontend.API.Controllers.ProductManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IControllerHelper _controllerHelper;

        public ImagesController(IHttpClientFactory clientFactory, IControllerHelper controllerHelper)
        {
            _clientFactory = clientFactory;
            _controllerHelper = controllerHelper;
        }

        [HttpGet("{id}", Name = "GetImageForDisplay")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(File))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageForDisplay(int id)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, $"images/{id}");


            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseImageResult(incomingHttpResponseMessage, Response);
        }


        [HttpGet("asBase64/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImageAsBase64String(int id)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, $"images/asBase64/{id}");

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseImageResult(incomingHttpResponseMessage, Response);
        }


        [Authorize(Roles = "Manager,Owner")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateImage(ImageForCreateDto imageForCreateDto)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Post, "images")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(imageForCreateDto)
            };

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(incomingHttpResponseMessage, "GetImageForDisplay");
        }
    }   
}
