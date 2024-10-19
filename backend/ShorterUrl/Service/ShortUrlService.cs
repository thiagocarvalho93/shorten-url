using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using ShorterUrl.DTOs;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service;

public class ShortUrlService
{
    public readonly ShortUrlRepository _repository;
    private readonly IMemoryCache _cache;
    public ShortUrlService(ShortUrlRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<List<ShortUrlDAO>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _repository.GetPaginatedAsync(page, pageSize, cancellationToken);
    }

    public async Task<ShortUrlDAO> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var entity = await _cache.GetOrCreateAsync(token, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
            return await _repository.GetByTokenAsync(token, cancellationToken);
        });

        if (entity is not null)
            return entity;

        throw new KeyNotFoundException();
    }

    public async Task<ShortUrlDAO> InsertAsync(ShortUrlInsertRequestDTO request, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByUrlAsync(request.Url, cancellationToken);

        if (entity is not null)
            return entity;

        string token = GenerateRandomAlphanumericString();

        ShortUrlDAO model = new()
        {
            Id = 0,
            Token = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(1),
            Url = request.Url,
        };

        await _repository.AddAsync(model);

        return model;
    }

    private static string GenerateRandomAlphanumericString(int size = 5)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder result = new(size);

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            for (int i = 0; i < size; i++)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                result.Append(chars[(int)(num % (uint)chars.Length)]);
            }
        }

        return result.ToString();
    }
}