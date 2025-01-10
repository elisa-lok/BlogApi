using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("[controller]")]

public class ConfigurationController(IConfiguration configuration) : ControllerBase
{
  [HttpGet]
  [Route("my-key")]
  public ActionResult GetMyKey()
  {
    var myKey = configuration["MyKey"];
    return Ok(myKey);
  }

  
    [HttpGet]
    [Route("database-configuration")]
    public ActionResult GetDatabaseConfiguration()
    {
        var type = configuration["database:Type"];
        var connectionString = configuration["Database:ConnectionStrings:DefaultConnection"]; // 访问具体的连接字符串
        if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(connectionString))
        {
          return BadRequest("Database configuration is missing or incomplete.");
        }
        return Ok(new { Type = type, ConnectionString = connectionString });
    }
}