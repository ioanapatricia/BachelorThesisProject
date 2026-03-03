using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BackendForFrontend.API.Dtos;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using BackendForFrontend.API.Services.BackendForFrontend;
using BackendForFrontend.API.Services.OrdersManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ordering.Contracts.Dtos;
using Ordering.Contracts.Models;

namespace BackendForFrontend.API.Controllers.OrdersManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IUsersService _usersService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IControllerHelper _controllerHelper;

        public OrdersController(
                IOrdersService ordersService, 
                IUsersService usersService,
                IHttpClientFactory clientFactory, 
                IControllerHelper controllerHelper
            )
        {
            _ordersService = ordersService;
            _usersService = usersService;
            _clientFactory = clientFactory;
            _controllerHelper = controllerHelper;
        }

        [Authorize(Roles = "Manager,Owner,Cook,Driver")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetOrders()
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.OrderingApi);
            var request = new HttpRequestMessage(HttpMethod.Get, "orders");

            var response = await client.SendAsync(request);

            return await _controllerHelper  
                .ParseActionResult(response);   
        }


        [Authorize(Roles = "Manager,Owner,Cook,Driver")]
        [HttpGet("{id}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderForGetDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]   
        public async Task<IActionResult> GetOrder(string id)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.OrderingApi);
            var request = new HttpRequestMessage(HttpMethod.Get, $"orders/{id}");

            var response = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(response);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderForGetDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(BffOrderForCreateDto orderForCreateDto)
        {
            // user
            var userId = User.GetUserId();
            var user = await _usersService.GetUser(userId);
                
            // filteredProducts
            var filteredProductsFromPmApiResponse = await GetFilteredProductsFromPmApi(orderForCreateDto.ProductFilters);

            if ((int)filteredProductsFromPmApiResponse.StatusCode != StatusCodes.Status200OK)
                return await _controllerHelper.ParseActionResult(filteredProductsFromPmApiResponse);

            var productForGetDtoListAsJson = await filteredProductsFromPmApiResponse.Content.ReadAsStringAsync();
            var productForGetDtoList = JsonConvert.DeserializeObject<List<global::ProductManagement.Contracts.Dtos.ProductForGetDto>>(productForGetDtoListAsJson);

            // normalize data  
            var normalizedOrder = _ordersService.NormalizeOrder(user, orderForCreateDto.PaymentTypeId, productForGetDtoList);

            // call orders.api
            var result = await CreateOrderInOrdersApi(normalizedOrder);

            return await _controllerHelper
                .ParseActionResult(result, "GetOrder");
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<HttpResponseMessage> GetFilteredProductsFromPmApi(IEnumerable<ProductFilter> filters)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var filteredProductsRequest = new HttpRequestMessage(HttpMethod.Get, "products/filtered")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(filters)
            };

            return await client.SendAsync(filteredProductsRequest);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<HttpResponseMessage> CreateOrderInOrdersApi(NormalizedOrderForCreationDto normalizedOrder)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.OrderingApi);
            var request = new HttpRequestMessage(HttpMethod.Post, "orders")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(normalizedOrder)
            };

            var result = await client.SendAsync(request);
            return result;
        }
    }
}
    