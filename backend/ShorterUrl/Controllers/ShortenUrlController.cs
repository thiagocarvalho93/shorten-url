using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShorterUrl.DTOs;
using ShorterUrl.Service;

namespace ShorterUrl.Controllers
{
    [ApiController]
    public class ShortenUrlController : ControllerBase
    {
        private readonly ShorUrlService _service;
        private readonly IMemoryCache _cache;

        public ShortenUrlController(ShorUrlService service, IMemoryCache cache)
        {
            _service = service;
            _cache = cache;
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
                var entity = await _cache.GetOrCreateAsync(token, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                    return await _service.GetByTokenAsync(token, cancellationToken);
                });
                return entity?.Url is null ? NotFound() : Redirect(entity.Url);
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
}