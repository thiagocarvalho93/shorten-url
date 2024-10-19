using Microsoft.AspNetCore.Mvc;
using ShorterUrl.DTOs;
using ShorterUrl.Service;

namespace ShorterUrl.Controllers;

[ApiController]
public class ShortUrlController : ControllerBase
{
    private readonly ShortUrlService _service;

    public ShortUrlController(ShortUrlService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 25, CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await _service.GetPaginatedAsync(page, pageSize, cancellationToken);
            return Ok(new { page, pageSize, data });
        }
        catch
        {
            return StatusCode(500, "Erro 5X0001");
        }
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> RedirectByTokenAsync([FromRoute] string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _service.GetByTokenAsync(token, cancellationToken);

            return result?.Url is null ? NotFound() : Redirect(result.Url);
        }
        catch
        {
            return StatusCode(500, "Erro 5X0002");
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> PostAsync([FromBody] ShortUrlInsertRequestDTO dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var created = await _service.InsertAsync(dto, cancellationToken);

            return Created($"{created.Token}", created);
        }
        catch
        {
            return StatusCode(500, "Erro 5X0003");
        }
    }
}