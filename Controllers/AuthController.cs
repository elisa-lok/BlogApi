using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BlogApi.Models;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private User? AuthenticateUser(string email, string password)
        {
            var fakeUser = new User
            {
                Id = 1,
                Name = "Test User",
                Email = "test@example.com",
                Password = "password123", 
                Role = "User",
                Status = "active"
            };

            if (fakeUser.Email == email && fakeUser.Password == password)
            {
                return fakeUser.Status == "active" ? fakeUser : null;
            }

            return null;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = AuthenticateUser(request.Email, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials or inactive account" });
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                userId = user.Id,
                userName = user.Name,
                userRole = user.Role
            });
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
