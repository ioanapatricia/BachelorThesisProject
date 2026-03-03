using System;
using System.Net.Http;
using System.Threading.Tasks;
using BackendForFrontend.API.Entities;
using BackendForFrontend.API.Extensions;
using BackendForFrontend.API.Helpers;
using BackendForFrontend.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public SeedController(
            IHttpClientFactory httpClientFactory, 
            DataContext context, 
            UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("bffSeed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SeedDataInBff()
        {
            try
            {
                await _context.Database.MigrateAsync();
                await DbInitializer.SeedData( _userManager, _roleManager, _context);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred during migration. Exception: {ex.Message}{Environment.NewLine}" +
                    $"Inner exception: {ex.InnerException}{Environment.NewLine}" +
                    $"Source: {ex.Source}{Environment.NewLine}" +
                    $"Stacktrace: {ex.StackTrace}{Environment.NewLine}");
            }

            return Ok("Database migrated successfully!");
        }


        [HttpPost("pmSeed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SeedDataInPm()
        {
            var client = _httpClientFactory.CreateClient(HttpClientsEnum.ProductManagementApi);
            var request = new HttpRequestMessage(HttpMethod.Post, "seed");

            var incomingHttpResponseMessage = await client.SendAsync(request);

            return (int) incomingHttpResponseMessage.StatusCode == StatusCodes.Status200OK 
                ? Ok(await incomingHttpResponseMessage.Content.ReadAsStringAsync()) 
                : StatusCode(StatusCodes.Status500InternalServerError, await incomingHttpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}
