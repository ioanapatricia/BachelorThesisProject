using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using BackendForFrontend.API.Dtos;
using BackendForFrontend.API.Entities;
using BackendForFrontend.API.Services.BackendForFrontend;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.API.Controllers.BackendForFrontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RegistrationController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegistrationDto registerDto)
        {
            if (await UserExists(registerDto.Username))
                return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
                return BadRequest(result.Errors);

            var userForReturn = new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
            return Ok(userForReturn);
        }

        private async Task<bool> UserExists(string username)
            => await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
