using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Contracts.Dtos;

namespace BackendForFrontend.API.Controllers.OrdersManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentTypesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IControllerHelper _controllerHelper;

        public PaymentTypesController(IHttpClientFactory httpClientFactory, IControllerHelper controllerHelper)
        {
            _httpClientFactory = httpClientFactory;
            _controllerHelper = controllerHelper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PaymentTypeForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var client = _httpClientFactory.CreateClient(HttpClientsEnum.OrderingApi);
            var request = new HttpRequestMessage(HttpMethod.Get, "paymentTypes");

            var response = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(response);
        }
    }
}
