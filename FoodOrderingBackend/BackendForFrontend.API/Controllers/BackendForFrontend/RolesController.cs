using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendForFrontend.API.Dtos;
using BackendForFrontend.API.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.API.Controllers.BackendForFrontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public RolesController(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }


        [Authorize(Roles = "Owner,Manager")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeRoleDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _dataContext.Roles
                .Where(x => x.Name.ToLower() != "member")
                .ToListAsync();

            if (!roles.Any())
                return NoContent();

            var rolesForReturn = _mapper.Map<IEnumerable<EmployeeRoleDto>>(roles);

            return Ok(rolesForReturn);
        }
    }
}
