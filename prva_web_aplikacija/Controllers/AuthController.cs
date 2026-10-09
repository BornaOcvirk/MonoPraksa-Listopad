using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prva_web_aplikacija.Auth;
using prva_web_aplikacija.Model;
using prva_web_aplikacija.Service.Common;

namespace prva_web_aplikacija.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        // POST api/auth/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
        {
            var created = await _userService.CreateAsync(dto);
            if (created == null)
                return Conflict("Username is already taken.");

            return Ok(_tokenService.CreateToken(created));
        }

        // POST api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _userService.LoginAsync(dto);
            if (user == null)
                return Unauthorized("Wrong username or password.");

            return Ok(_tokenService.CreateToken(user));
        }

        // GET api/auth/me  — who am I, according to my token?
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = User.FindFirst("sub")?.Value,
                Username = User.FindFirst("name")?.Value,
                Role = User.FindFirst("role")?.Value
            });
        }
    }
}