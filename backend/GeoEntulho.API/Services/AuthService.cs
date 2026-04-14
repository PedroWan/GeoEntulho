using GeoEntulho.API.Data;
using GeoEntulho.API.DTOs;
using GeoEntulho.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace GeoEntulho.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Register(RegisterDto dto);
        Task<AuthResponseDto> Login(LoginDto dto);
        Task<(bool, string)> ValidatePassword(string password, string hash);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> Register(RegisterDto dto)
        {
            // Validar entrada
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email e Senha são obrigatórios"
                };
            }

            if (dto.Password.Length < 6)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Senha deve ter no mínimo 6 caracteres"
                };
            }

            // Verificar se usuario ja existe
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email já registrado"
                };
            }

            // Validar tipo de usuario
            if (dto.Type != "citizen" && dto.Type != "company")
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Tipo de usuário inválido (citizen ou company)"
                };
            }

            try
            {
                // Hash da senha
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                // Criar novo usuario
                var user = new User
                {
                    Email = dto.Email.ToLower(),
                    PasswordHash = passwordHash,
                    Name = dto.Name ?? "Usuário",
                    Type = dto.Type,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Usuário registrado com sucesso"
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Erro ao registrar: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponseDto> Login(LoginDto dto)
        {
            // Validar entrada
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email e Senha são obrigatórios"
                };
            }

            try
            {
                // Buscar usuario
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email.ToLower());
                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email ou senha inválidos"
                    };
                }

                // Verificar senha
                var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
                if (!isValidPassword)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email ou senha inválidos"
                    };
                }

                // Gerar JWT token
                var token = GenerateJwtToken(user);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login bem-sucedido",
                    Token = token,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        Type = user.Type
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Erro ao fazer login: {ex.Message}"
                };
            }
        }

        public async Task<(bool, string)> ValidatePassword(string password, string hash)
        {
            try
            {
                var isValid = BCrypt.Net.BCrypt.Verify(password, hash);
                return (isValid, isValid ? "Senha válida" : "Senha inválida");
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao validar: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
            var jwtAudience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
            var jwtDurationMinutes = int.Parse(_configuration["Jwt:DurationMinutes"] ?? "1440");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Type", user.Type)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtDurationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
