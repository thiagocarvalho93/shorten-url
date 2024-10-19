using ShorterUrl.DTOs;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service
{
    public class ShorUrlService
    {
        public readonly ShortUrlRepository _repository;

        public ShorUrlService(ShortUrlRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ShortUrl>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return await _repository.GetPaginatedAsync(page, pageSize, cancellationToken);
        }

        public async Task<ShortUrl?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByTokenAsync(token, cancellationToken);
        }

        public async Task<ShortUrl> InsertAsync(ShortUrlInsertRequestDTO request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByUrlAsync(request.Url, cancellationToken);

            if (entity is not null)
                return entity;

            string token = RandomTokenService.GenerateRandomAlphanumericString();

            ShortUrl model = new()
            {
                Id = 0,
                Token = token,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(1),
                Url = entity?.Url
            };
            await _repository.AddAsync(model);

            return model;
        }
    }
}