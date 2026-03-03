using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BackendForFrontend.API.Dtos;
using BackendForFrontend.API.Entities;
using BackendForFrontend.API.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
    
namespace BackendForFrontend.API.Controllers.BackendForFrontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public EmployeesController(UserManager<AppUser> userManager, DataContext dataContext, IMapper mapper)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _mapper = mapper;
        }


        [Authorize(Roles = "Manager,Owner")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeForCreateOrUpdateDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var memberRole = await _dataContext.Roles.FirstAsync(r => r.Name.ToLower().Equals("member"));

            var employees = await _dataContext.Users
                .Include(u => u.Address)
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                    .Select(u => new
                    {
                        u.Id,
                        Username = u.UserName,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.PhoneNumber,
                        u.Address,

                        Role = u.UserRoles.Select(r => new {r.Role.Id, r.Role.Name}).FirstOrDefault()
                    })
                .ToListAsync();

            if (!employees.Any())
                return NoContent();


            foreach (var employee in employees.ToList())
            {
                if (employee.Role.Equals(new {memberRole.Id, memberRole.Name}))
                {
                    employees.Remove(employee);
                }
            }
     
            return Ok(employees);
        }


        [Authorize(Roles = "Manager,Owner")]
        [HttpGet("{id}", Name = "GetEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeForCreateOrUpdateDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var memberRole = await _dataContext.Roles.FirstAsync(r => r.Name.ToLower().Equals("member"));

            var employee = await _dataContext.Users
                .Include(u => u.Address)
                .Include(r => r.UserRoles)
                .ThenInclude(r => r.Role)
                .Select(u => new
                {
                    u.Id,
                    Username = u.UserName,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.PhoneNumber,
                    u.Address,

                    Role = u.UserRoles.Select(r => new { r.Role.Id, r.Role.Name }).FirstOrDefault()
                })
                .FirstOrDefaultAsync(x => x.Id == id);

            if (employee is null)
                return NotFound("No employee has been found with this id.");

      
            if (employee.Role.Equals(new { memberRole.Id, memberRole.Name }))
                return NotFound("No employee has been found with this id.");


            return Ok(employee);
        }


        [Authorize(Roles = "Owner")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeForCreateOrUpdateDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] EmployeeForCreateOrUpdateDto employeeDto)
        {
            if (await UserExists(employeeDto.Username))
                return BadRequest("Username is taken");

            var memberRole = await _dataContext.Roles.FirstOrDefaultAsync(r => r.Id == employeeDto.RoleId);
            if (memberRole is null)
                return BadRequest("Role does not exist");


            var user = _mapper.Map<AppUser>(employeeDto);

            var result = await _userManager.CreateAsync(user, employeeDto.Password);

            if (result.Succeeded)
                await _dataContext.UserRoles.AddAsync(new AppUserRole {UserId = user.Id, RoleId = memberRole.Id});

            var roleResult = await _dataContext.SaveChangesAsync();
            return roleResult > 0 ? Ok(new { id = user.Id }) : StatusCode(StatusCodes.Status500InternalServerError, "unknown exception occurred");
        }


        [Authorize(Roles = "Owner")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeForCreateOrUpdateDto employeeDto)
        {
            var newUserRole = await _dataContext.Roles.FirstOrDefaultAsync(r => r.Id == employeeDto.RoleId);
            if (newUserRole is null)
                return BadRequest("Role does not exist");

            var userFromDb = await _userManager.Users
                .Include(us => us.Address)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userFromDb is null)
                return NotFound();

            _mapper.Map(employeeDto, userFromDb);

            var result = await _userManager.UpdateAsync(userFromDb);

            if (result.Succeeded)
            {

                var userRoles = await _userManager.GetRolesAsync(userFromDb);
                result = await _userManager.RemoveFromRolesAsync(userFromDb, userRoles);
                result = await _userManager.AddToRoleAsync(userFromDb, newUserRole.Name);
            }


            return result.Succeeded ? NoContent() : StatusCode(StatusCodes.Status500InternalServerError, "unknown exception occurred");
        }


        [Authorize(Roles = "Owner")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var userFromDb = await _userManager.Users
                .Include(us => us.Address)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userFromDb is null)
                return NotFound();

            var oldAddress = await _dataContext.Addresses
                .FirstOrDefaultAsync(ad => ad.Id == userFromDb.Address.Id);

            _dataContext.Addresses.Remove(oldAddress);
            await _dataContext.SaveChangesAsync();
            await _userManager.DeleteAsync(userFromDb);

            return NoContent();
        }


        private async Task<bool> UserExists(string username)
            => await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
