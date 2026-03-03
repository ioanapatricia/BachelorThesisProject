using System.Threading.Tasks;
using BackendForFrontend.API.Dtos;
using BackendForFrontend.API.Entities;
using BackendForFrontend.API.Services.BackendForFrontend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendForFrontend.API.Controllers.BackendForFrontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthenticationController(
                UserManager<AppUser> userManager, 
                SignInManager<AppUser> signInManager, 
                ITokenService tokenService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            var user = await _userManager.Users
                .SingleOrDefaultAsync(usr => usr.UserName == loginDto.Username.ToLower());

            if (user == null) 
                return Unauthorized("Invalid username.");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) 
                return Unauthorized("Invalid password.");

            var userForReturn = new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };

            return Ok(userForReturn);
        }
    }
}
    