using BlogApi.Data;
using Microsoft.AspNetCore.Mvc;

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
  }
}