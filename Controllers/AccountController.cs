using BlogApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]

  public class AccountController : ControllerBase
  { 
    private readonly BlogDbContext _context;

    public AccountController(BlogDbContext context)
    {
      _context = context;
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
  } 
}