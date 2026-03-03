using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using SiteManagement.API.Entities;
using SiteManagement.API.Services.Interfaces;
using SiteManagement.Contracts.Dtos;

namespace SiteManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IMapper _mapper;

        public SettingsController(ISettingsService settingsService, IMapper mapper)
        {
            _settingsService = settingsService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SiteSettingsForReturnDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetSettings()
        {
            var settings = await _settingsService.GetAsync();

            if (settings is null)
                return NoContent();

            var settingsForReturn = _mapper.Map<SiteSettingsForReturnDto>(settings);

            return Ok(settingsForReturn);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(SiteSettingsForUpdateDto settingsForUpdateDto)
        {
            var settingsFromRepo = await _settingsService.GetAsync();

            if (settingsFromRepo is null)
                return NotFound("The settings you're looking for were not found");


            var settingsForUpdate = _mapper.Map<SiteSettings>(settingsForUpdateDto);

            var result = await _settingsService.UpdateAsync(settingsFromRepo.Id, settingsForUpdate);

            if (result.IsFailure)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Error);

            return NoContent();
        }
    }
}
