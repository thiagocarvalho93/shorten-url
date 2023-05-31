using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShorterUrl.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class ShortenUrlController : ControllerBase
    {
        [HttpGet("{token}")]
        public async Task<IActionResult> Get([FromRoute] string token)
        {
            return Ok("token");
        }
    }
}