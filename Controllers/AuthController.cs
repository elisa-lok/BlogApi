using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using BlogApi.Data;

namespace BlogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BlogDbContext _context;

        public AuthController(IConfiguration configuration, BlogDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        private async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user.Status == "active" ? user : null;
            }

            return null;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await AuthenticateUserAsync(request.Email, request.Password);

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


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            return BadRequest(new { message = "Email is already registered." });
        }

        var newUser = new User
        { 
            Name = request.Name,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password), 
            Role = "User",
            Status = "active",
            Country = "New Zealand",
            PhoneNumber = request.PhoneNumber,
        };

        _context.Users.Add(newUser);

        try
        {
            await _context.SaveChangesAsync();
            var userResponse = new 
            {
                userId = newUser.Id,
                userName = newUser.Name,
                userRole = newUser.Role
            };
            return Ok(new { message = "Registration successful!", user = userResponse });
        }
        catch (Exception ex)
        {
        return StatusCode(500, new { message = "An error occurred while saving data.", error = ex.Message });
        }
     }

        private string GenerateJwtToken(User user)
        {   
            var key = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key is not configured.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
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

    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
