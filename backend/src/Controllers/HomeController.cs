using Microsoft.AspNetCore.Mvc;

namespace ShorterUrl.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("health-check")]
    public IActionResult Get() => Ok("Healthy");
}
