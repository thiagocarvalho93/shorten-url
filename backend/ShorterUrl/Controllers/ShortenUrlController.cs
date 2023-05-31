using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ShorterUrl.DTOs;
using ShorterUrl.Models;
using ShorterUrl.Repository;
using ShorterUrl.Service;

namespace ShorterUrl.Controllers
{
    [ApiController]
    public class ShortenUrlController : ControllerBase
    {
        private readonly ShortUrlRepository _repository;
        private readonly IMemoryCache _cache;

        public ShortenUrlController(ShortUrlRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAsync(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 25)
        {
            try
            {
                var data = await _repository.GetPaginatedAsync(page, pageSize);
                return Ok(new
                {
                    page,
                    pageSize,
                    data
                });
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Erro 5X0001");
            }
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> RedirectByTokenAsync([FromRoute] string token)
        {
            try
            {
                var entity = await _cache.GetOrCreateAsync(token, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
                    return await _repository.GetByTokenAsync(token);
                });
                return entity is null ? NotFound() : Redirect(entity.Url);
            }
            catch
            {
                return StatusCode(500, "Erro 5X0002");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromBody] AddShortUrlDTO dto)
        {
            try
            {
                var entity = await _repository.GetByUrlAsync(dto.Url);
                if (entity is not null)
                    return Ok(entity);

                string token = RandomTokenService.GenerateRandomAlfanumericString();
                ShortenUrl model = new()
                {
                    Id = 0,
                    Token = token,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddDays(1),
                    Url = dto.Url
                };
                await _repository.AddAsync(model);

                return Created($"{token}", model);
            }
            catch
            {
                return StatusCode(500, "Erro 5X0003");
            }
        }

    }
}