using GeoEntulho.API.DTOs;
using GeoEntulho.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GeoEntulho.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(IFirebaseService firebaseService, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _firebaseService = firebaseService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Register a new user (citizen or company)
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Registration attempt for email: {dto.Email}");

                // Criar usuário no Firebase
                var userId = await _firebaseService.CreateUserAsync(dto.Email, dto.Password, dto.Name, dto.Type);

                // Gerar JWT token
                var token = GenerateJwtToken(userId, dto.Email, dto.Name, dto.Type);

                var response = new AuthResponseDto
                {
                    Success = true,
                    Message = "Usuário registrado com sucesso",
                    Token = token,
                    User = new UserDto
                    {
                        Id = int.Parse(userId.GetHashCode().ToString()),
                        Email = dto.Email,
                        Name = dto.Name,
                        Type = dto.Type
                    }
                };

                _logger.LogInformation($"User registered successfully: {dto.Email}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration error: {ex.Message}");
                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = $"Erro ao registrar: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Login attempt for email: {dto.Email}");

                // Verificar credenciais com Firebase
                var user = await _firebaseService.GetUserAsync(dto.Email);

                if (user == null)
                {
                    return Unauthorized(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email ou senha inválidos"
                    });
                }

                // Gerar JWT token
                var token = GenerateJwtToken(
                    dto.Email,
                    dto.Email,
                    user["name"].ToString(),
                    user["type"].ToString()
                );

                var response = new AuthResponseDto
                {
                    Success = true,
                    Message = "Login realizado com sucesso",
                    Token = token,
                    User = new UserDto
                    {
                        Id = dto.Email.GetHashCode(),
                        Email = dto.Email,
                        Name = user["name"].ToString(),
                        Type = user["type"].ToString()
                    }
                };

                _logger.LogInformation($"User logged in successfully: {dto.Email}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = $"Erro ao fazer login: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// Get current user info (requires authentication)
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            var nameClaim = User.FindFirst(ClaimTypes.Name);
            var typeClaim = User.FindFirst("Type");

            if (emailClaim == null)
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            var user = new UserDto
            {
                Id = emailClaim.Value.GetHashCode(),
                Email = emailClaim.Value,
                Name = nameClaim?.Value ?? "",
                Type = typeClaim?.Value ?? ""
            };

            return Ok(new { success = true, user });
        }

        /// <summary>
        /// Get user profile from Firestore
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email);
            if (emailClaim == null)
            {
                return Unauthorized(new { message = "Token inválido" });
            }

            try
            {
                var user = await _firebaseService.GetUserAsync(emailClaim.Value);
                if (user == null)
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting profile: {ex.Message}");
                return StatusCode(500, new { message = "Erro ao obter perfil" });
            }
        }

        /// <summary>
        /// Helper: Generate JWT Token
        /// </summary>
        private string GenerateJwtToken(string userId, string email, string name, string type)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(
                Environment.GetEnvironmentVariable("JWT_SECRET") ?? 
                jwtSettings["Key"] ?? "default-secret-key-change-this"
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name),
                    new Claim("Type", type)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = jwtSettings["Issuer"] ?? "GeoEntulho",
                Audience = jwtSettings["Audience"] ?? "GeoEntulho.Users",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name),
                    new Claim("Type", type)
                }),
                Expires = DateTime.UtcNow.AddMinutes(1440),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
            return Ok(profile);
        }
    }
}
