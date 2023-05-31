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
        public async Task<IActionResult> GetAsync() => Ok(await _repository.GetAsync());

        [HttpGet("token/{token}")]
        public async Task<IActionResult> RedirectByTokenAsync([FromRoute] string token)
        {
            //TODO
            return Ok(await _repository.GetAsync());
        }

        [HttpPost("")]
        public async Task<IActionResult> PostAsync([FromBody] AddShortUrlDTO dto)
        {
            // verify if already exists
            var urlDb = await _repository.GetByLongUrlAsync(dto.LongUrl);
            if (urlDb is not null)
            {
                if (DateTime.Now <= urlDb.ExpiresAt)
                    return Ok(urlDb.Token);
            }

            // add new data
            string token = RandomTokenService.generateValue();
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

    }
}