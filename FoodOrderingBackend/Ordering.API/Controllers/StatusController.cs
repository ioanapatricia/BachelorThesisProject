using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Services.Interfaces;
using Ordering.Contracts.Dtos;

namespace Ordering.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IMapper _mapper;

        public StatusController(IStatusService statusService, IMapper mapper)
        {
            _statusService = statusService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StatusForGetDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var statusList = await _statusService.GetAllAsync();

            if (!statusList.Any())
                return NoContent();

            var statusListForReturn = _mapper.Map<IEnumerable<StatusForGetDto>>(statusList);

            return Ok(statusListForReturn);
        }
    }
}
