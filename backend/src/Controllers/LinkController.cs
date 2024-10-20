using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectByShortCodeAsync([FromRoute] string shortCode, CancellationToken cancellationToken = default)
    {
        var analytics = new ClickRequestDTO(HttpContext);

        var result = await _service.RedirectByShortCodeAsync(shortCode, analytics, cancellationToken);

        return Redirect(result.OriginalUrl);
    }

    [HttpGet("links")]
    [Authorize]
    public async Task<IActionResult> GetAsync([FromQuery] PaginatedRequestDTO request, CancellationToken cancellationToken = default)
    {
        var data = await _service.GetPaginatedAsync(request.Page, request.PageSize, cancellationToken);

        return Ok(data);
    }

    [HttpPost("links")]
    [Authorize]
    public async Task<IActionResult> PostAsync([FromBody] LinkInsertRequestDTO dto, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var created = await _service.InsertAsync(dto, userId, cancellationToken);

        return Created($"{created.ShortCode}", created);
    }

    [HttpGet("links/{shortCode}")]
    [Authorize]
    public async Task<IActionResult> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        var data = await _service.GetByShortCodeAsync(shortCode, cancellationToken);

        return Ok(data);
    }

    [HttpDelete("links/{shortCode}")]
    [Authorize]
    public async Task<IActionResult> DeleteByShortCodeAsync([FromRoute] string shortCode, CancellationToken cancellationToken = default)
    {
        await _service.DeleteByShortCodeAsync(shortCode, cancellationToken);

        return NoContent();
    }

    [HttpPatch("links/alias/{shortCode}")]
    [Authorize]
    public async Task<IActionResult> ChangeAlias([FromRoute] string shortCode, [FromBody] LinkChangeAliasRequestDTO dto, CancellationToken cancellationToken = default)
    {
        await _service.ChangeAlias(shortCode, dto.Alias, cancellationToken);

        return NoContent();
    }
}