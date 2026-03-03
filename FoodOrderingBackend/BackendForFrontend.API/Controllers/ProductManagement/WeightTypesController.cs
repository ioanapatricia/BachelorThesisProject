using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Contracts.Dtos;

namespace BackendForFrontend.API.Controllers.ProductManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightTypesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IControllerHelper _controllerHelper;

        public WeightTypesController(IHttpClientFactory clientFactory, IControllerHelper controllerHelper)
        {
            _clientFactory = clientFactory;
            _controllerHelper = controllerHelper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeightTypeDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetWeightTypes()
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, "weightTypes");

            var response = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(response);
        }
    }
}
