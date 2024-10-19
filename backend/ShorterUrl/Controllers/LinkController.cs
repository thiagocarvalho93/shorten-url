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

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectByShortCodeAsync([FromRoute] string shortCode, CancellationToken cancellationToken = default)
    {
        var analytics = new ClickRequestDTO(HttpContext);

        var result = await _service.RedirectByShortCodeAsync(shortCode, analytics, cancellationToken);

        return Redirect(result.OriginalUrl);
    }

    [HttpPost("")]
    public async Task<IActionResult> PostAsync([FromBody] LinkInsertRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var created = await _service.InsertAsync(dto, cancellationToken);

        return Created($"{created.ShortCode}", created);
    }

    [HttpDelete("id/{id}")]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        await _service.DeleteByIdAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{shortCode}")]
    public async Task<IActionResult> DeleteByShortCodeAsync([FromRoute] string shortCode, CancellationToken cancellationToken = default)
    {
        await _service.DeleteByShortCodeAsync(shortCode, cancellationToken);

        return NoContent();
    }
}