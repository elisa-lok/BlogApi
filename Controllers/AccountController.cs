using BlogApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace BlogApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]

  public class AccountController : ControllerBase
  { 
    private readonly BlogDbContext _context;
    private readonly IConfiguration _configuration;

    public AccountController(BlogDbContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }

    [HttpPut("UpdateUserName")]
    public async Task<IActionResult> UpdateUserName(int id, string newUserName)
    {
      var user = _context.Users.FirstOrDefault(u => u.Id == id);
      if (user == null)
      {
       return NotFound(new { Message = "User not found" });
      }

      user.Name = newUserName;
      await _context.SaveChangesAsync();

      return Ok(new { Message = "Username updated successfully" });
    }

    [HttpPut("UpdatePhoneNumber")]
    public async Task<IActionResult> UpdatePhoneNumber(int id, string newPhoneNumber)
    {
      var user = _context.Users.FirstOrDefault(u => u.Id == id);
      if (user == null)
      {
       return NotFound(new { Message = "User not found" });
      }

      user.PhoneNumber = newPhoneNumber;
      await _context.SaveChangesAsync();

      return Ok(new { Message = "PhoneNumber updated successfully" });
    }

    [HttpPut("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword(int id, string currentPassword, string newPassword)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
      if (user == null)
      {
        return NotFound(new { Message = "User not found" });
      }

      if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
      {
        return BadRequest(new { Message = "Your password is incorrect" });
      }

    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
    await _context.SaveChangesAsync();

    return Ok(new { Message = "Password updated successfully" });
    }

    [HttpPut("UpdateEmail")]
    public async Task<IActionResult> UpdateEmail(int id, string newEmail)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
      if (user == null)
      {
        return NotFound(new { Message = "User not found" });
      }

    
      if (await _context.Users.AnyAsync(u => u.Email == newEmail))
      {
        return BadRequest(new { Message = "Email has been used already" });
      }

      user.Email = newEmail;
      await _context.SaveChangesAsync();

      return Ok(new { Message = "Email updated successfully" });
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
      if (user == null)
      {
        return NotFound(new { Message = "User not found" });
      }

      string resetToken = GenerateResetToken(email);
      string resetLink = $"http://myPersonalBlog.com/reset-password?token={resetToken}";

      SendResetEmail(user.Email, resetLink);

      return Ok(new { Message = "Password reset email sent" });
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(string token, string newPassword)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_configuration["My_Key"] ?? throw new ArgumentNullException("My_Key is missing from configuration")); 

      try
      {
        var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var email = claims.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
          return NotFound(new { Message = "User not found" });
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Password reset successfully" });
      }
      catch
      {
        return BadRequest(new { Message = "Invalid or expired token" });
      }
    }

    [HttpDelete("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount(int id)
    { 
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
      if (user == null)
      {
        return NotFound(new { Message = "User not found" });
      }
      //_context.Users.Remove(user);
      user.Status = "deleted";
      await _context.SaveChangesAsync();

      return Ok(new { Message = "Account status updated to deleted" });
    }

    private string GenerateResetToken(string email)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Your_Secret_Key"));
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, email),
        new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddHours(1).ToString())  // 1小时过期
    };

    var token = new JwtSecurityToken(
        issuer: "your-app",
        audience: "your-app",
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private void SendResetEmail(string email, string resetLink)
  {
    try
    {
        var fromAddress = new MailAddress("15377821@qq.com", "My blog");
        var toAddress = new MailAddress(email);
        const string subject = "Password Reset Request";
        string body = $"Click the link below to reset your password:\n{resetLink}";

        var smtp = new SmtpClient
        {
            Host = "smtp.qq.com",
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential("14672288@qq.com", "123456")
        };

        using (var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        })
        {
            smtp.Send(message);
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Failed to send email: {ex.Message}");
      }
    }
  } 
}