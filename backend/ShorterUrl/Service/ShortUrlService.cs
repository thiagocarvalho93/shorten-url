using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using ShorterUrl.DTOs;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service;

public class ShortUrlService
{
    public readonly ShortUrlRepository _urlRepository;
    public readonly AnalyticsRepository _analyticsRepository;
    private readonly IMemoryCache _cache;
    public ShortUrlService(ShortUrlRepository repository, AnalyticsRepository analyticsRepository, IMemoryCache cache)
    {
        _urlRepository = repository;
        _analyticsRepository = analyticsRepository;
        _cache = cache;
    }

    public async Task<List<ShortUrlDAO>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _urlRepository.GetPaginatedAsync(page, pageSize, cancellationToken);
    }

    public async Task<ShortUrlDAO> GetByTokenAsync(string token, AnalyticsRequestDTO? analytics = null, CancellationToken cancellationToken = default)
    {
        var shortUrl = await _cache.GetOrCreateAsync(token, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
            return await _urlRepository.GetByTokenAsync(token, cancellationToken);
        });

        if (shortUrl is not null)
        {
            var analyticsDAO = new AnalyticsDAO()
            {
                ShortUrlId = shortUrl.Id,
                ClickDate = DateTime.Now,
                IpAdress = analytics?.IpAdress ?? "",
                Location = analytics?.Location ?? "",
                Referrer = analytics?.Referrer ?? "",
                UserAgent = analytics?.UserAgent ?? ""
            };
            await _analyticsRepository.AddAsync(analyticsDAO);

            return shortUrl;
        }

        throw new KeyNotFoundException();
    }

    public async Task<ShortUrlDAO> InsertAsync(ShortUrlInsertRequestDTO request, CancellationToken cancellationToken = default)
    {
        var entity = await _urlRepository.GetByUrlAsync(request.Url, cancellationToken);

        if (entity is not null)
            return entity;

        string token = GenerateRandomAlphanumericString();

        ShortUrlDAO model = new()
        {
            Id = 0,
            ShortCode = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(1),
            OriginalUrl = request.Url,
        };

        await _urlRepository.AddAsync(model);

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