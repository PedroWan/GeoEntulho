using GeoEntulho.API.DTOs;
using GeoEntulho.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoEntulho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user (citizen or company)
        /// </summary>
        /// <param name="dto">Registration data</param>
        /// <returns>Success or error message</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Registration attempt for email: {dto.Email}");

            var result = await _authService.Register(dto);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            _logger.LogInformation($"User registered successfully: {dto.Email}");

            return Ok(result);
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        /// <param name="dto">Login credentials</param>
        /// <returns>JWT token and user info if successful</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Login attempt for email: {dto.Email}");

            var result = await _authService.Login(dto);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            _logger.LogInformation($"User logged in successfully: {dto.Email}");

            return Ok(result);
        }

        /// <summary>
        /// Get current user info (requires authentication)
        /// </summary>
        /// <returns>Current user details</returns>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            var emailClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var nameClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Name);
            var typeClaim = User.FindFirst("Type");

            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var user = new UserDto
            {
                Id = int.Parse(userIdClaim.Value),
                Email = emailClaim?.Value ?? "",
                Name = nameClaim?.Value ?? "",
                Type = typeClaim?.Value ?? ""
            };

            return Ok(new { success = true, user });
        }
    }
}
