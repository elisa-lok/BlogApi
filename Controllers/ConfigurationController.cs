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
}