using System.Net.Http;
using System.Threading.Tasks;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Contracts.Dtos;

namespace BackendForFrontend.API.Controllers.SiteManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IControllerHelper _controllerHelper;

        public SettingsController(IHttpClientFactory clientFactory, IControllerHelper controllerHelper)
        {
            _clientFactory = clientFactory;
            _controllerHelper = controllerHelper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SiteSettingsForReturnDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetSettings()
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.SiteManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Get, "settings");

            var response = await client.SendAsync(request);

            return await _controllerHelper.ParseActionResult(response);
        }

        [Authorize(Roles = "Manager,Owner")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(SiteSettingsForUpdateDto siteSettingsForUpdateDto)
        {
            var client = _clientFactory.CreateClient(HttpClientsEnum.SiteManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Put, "settings")
            {
                Content = _controllerHelper
                    .GetObjectAsStringContent(siteSettingsForUpdateDto)
            };

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return await _controllerHelper
                .ParseActionResult(incomingHttpResponseMessage);
        }
    }
}
