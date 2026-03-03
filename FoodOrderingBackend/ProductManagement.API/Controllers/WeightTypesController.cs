using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProductManagement.API.Services.Interfaces;
using ProductManagement.Contracts.Dtos;

namespace ProductManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeightTypesController : ControllerBase
    {
        private readonly IWeightTypesService _weightTypesService;
        private readonly IMapper _mapper;

        public WeightTypesController(IWeightTypesService weightTypesService, IMapper mapper)
        {
            _weightTypesService = weightTypesService;
            _mapper = mapper;

        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeightTypeDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetWeightTypes()
        {
            var weightTypes = await _weightTypesService.GetWeightTypesAsync();

            if (!weightTypes.Any())
                return NoContent();

            var weightTypesForReturn = _mapper.Map<IEnumerable<WeightTypeDto>>(weightTypes);

            return Ok(weightTypesForReturn);
        }
    }
}
