using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Contracts.Dtos;
using ProductManagement.Contracts.Dtos.CategoryDtos;

namespace BackendForFrontend.API.Controllers.ProductManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase  
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IControllerHelper _controllerHelper;

        public CategoriesController(IHttpClientFactory clientFactory, IControllerHelper controllerHelper)
        {
            _clientFactory = clientFactory;
            _controllerHelper = controllerHelper;
        }

        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductCategoryForGetDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategory(int id)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, $"categories/{id}");

            var response = await client.SendAsync(request);

            return await _controllerHelper.ParseActionResult(response);

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductCategoryForListDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, "categories");

            var response = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(response);
        }



        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryWithProductsDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllWithProducts()
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, "categories/products");

            var response = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(response);
        }

        [Authorize(Roles = "Manager,Owner")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductCategoryForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(ProductCategoryForCreateOrUpdateDto categoryForCreateDto)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Post, "categories")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(categoryForCreateDto)
            };

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(incomingHttpResponseMessage, "GetCategory");
        }

        [Authorize(Roles = "Manager,Owner")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, ProductCategoryForCreateOrUpdateDto categoryForUpdateDto)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Put, $"categories/{id}")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(categoryForUpdateDto)
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
            var request = new HttpRequestMessage(HttpMethod.Delete, $"categories/{id}");

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(incomingHttpResponseMessage);
        }
    }
}
