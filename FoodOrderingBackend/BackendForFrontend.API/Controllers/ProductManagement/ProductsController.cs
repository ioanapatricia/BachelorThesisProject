using System.Collections.Generic;
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
    public class ProductsController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IControllerHelper _controllerHelper;

        public ProductsController(IHttpClientFactory clientFactory, IControllerHelper controllerHelper)
        {
            _clientFactory = clientFactory;
            _controllerHelper = controllerHelper;
        }


        [HttpGet("{id}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductForGetDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct(int id)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, $"products/{id}");

            var response = await client.SendAsync(request);
            
            return await _controllerHelper
                .ParseActionResult(response);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetProducts()
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, "products");

            var response = await client.SendAsync(request);
                
            return await _controllerHelper
                .ParseActionResult(response);
        }

        [Authorize(Roles = "Manager,Owner")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ProductForCreateOrUpdateDto productForCreateDto)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Post, "products")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(productForCreateDto)
            };

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(incomingHttpResponseMessage, "GetProduct");
        }

        [Authorize(Roles = "Manager,Owner")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, ProductForCreateOrUpdateDto productForCreateDto)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Put, $"products/{id}")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(productForCreateDto)
            };

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(incomingHttpResponseMessage);
        }

        [Authorize(Roles = "Manager,Owner")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Delete, $"products/{id}");

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(incomingHttpResponseMessage);
        }
    }
}
    