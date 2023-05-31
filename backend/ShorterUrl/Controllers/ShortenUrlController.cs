using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public ShortenUrlController(ShortUrlRepository repository)
        {
            _repository = repository;
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
                //TODO check cache
                var entity = await _repository.GetByTokenAsync(token);
                return entity is null ? NotFound() : Redirect(entity.LongUrl);
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
                var entity = await _repository.GetByLongUrlAsync(dto.LongUrl);
                if (entity is not null)
                {
                    if (DateTime.Now <= entity.ExpiresAt)
                        return Ok(entity.Token);
                }

                string token = RandomTokenService.generateRandomAlfanumericString();
                ShortenUrl model = new()
                {
                    Id = 0,
                    Token = token,
                    CreatedAt = DateTime.Now,
                    ExpiresAt = DateTime.Now.AddDays(1),
                    LongUrl = dto.LongUrl
                };
                var data = await _repository.AddAsync(model);

                return Created($"{token}", model);
            }
            catch
            {
                return StatusCode(500, "Erro 5X0003");
            }
        }

    }
}