using Microsoft.AspNetCore.Mvc;
using ShorterUrl.DTOs;
using ShorterUrl.Service;

namespace ShorterUrl.Controllers;

[ApiController]
public class LinkController : ControllerBase
{
    private readonly LinkService _service;

    public LinkController(LinkService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 25, CancellationToken cancellationToken = default)
    {
        var data = await _service.GetPaginatedAsync(page, pageSize, cancellationToken);

        return Ok(new { page, pageSize, data });
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> RedirectByTokenAsync([FromRoute] string token, CancellationToken cancellationToken = default)
    {
        var analytics = new AnalyticsRequestDTO(HttpContext);

        var result = await _service.RedirectByTokenAsync(token, analytics, cancellationToken);

        return Redirect(result.OriginalUrl);
    }

    [HttpPost("")]
    public async Task<IActionResult> PostAsync([FromBody] LinkInsertRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var created = await _service.InsertAsync(dto, cancellationToken);

        return Created($"{created.ShortCode}", created);
    }
}