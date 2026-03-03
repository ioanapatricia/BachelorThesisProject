using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Helpers;
using ProductManagement.API.Persistence;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly DataContext _context;

        public SeedController(DataContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult SeedData()
        {
            try
            {
                _context.Database.Migrate();
                DbInitializer.SeedData(_context);
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
    }
}
