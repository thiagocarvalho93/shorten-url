using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using ShorterUrl.DTOs;
using ShorterUrl.Exceptions;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service;

public class LinkService
{
    public readonly LinkRepository _urlRepository;
    public readonly AnalyticsRepository _analyticsRepository;
    private readonly IMemoryCache _cache;
    public LinkService(LinkRepository repository, AnalyticsRepository analyticsRepository, IMemoryCache cache)
    {
        _urlRepository = repository;
        _analyticsRepository = analyticsRepository;
        _cache = cache;
    }

    public async Task<List<LinkModel>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _urlRepository.GetPaginatedAsync(page, pageSize, cancellationToken);
    }

    public async Task<LinkModel> RedirectByTokenAsync(string token, AnalyticsRequestDTO? analytics, CancellationToken cancellationToken = default)
    {
        var link = await GetByTokenAsync(token, cancellationToken);

        var analyticsDAO = new AnalyticsModel()
        {
            LinkId = link.Id,
            ClickDate = DateTime.Now,
            IpAdress = analytics?.IpAdress ?? "",
            Location = analytics?.Location ?? "",
            Referrer = analytics?.Referrer ?? "",
            UserAgent = analytics?.UserAgent ?? ""
        };
        await _analyticsRepository.AddAsync(analyticsDAO, cancellationToken);

        return link;
    }

    public async Task<LinkModel> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var link = await _cache.GetOrCreateAsync(token, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
            return await _urlRepository.GetByTokenAsync(token, cancellationToken);
        });

        if (link is not null)
            return link;

        throw new NotFoundException($"Token {token} not found.");
    }

    public async Task<LinkModel> InsertAsync(LinkInsertRequestDTO request, CancellationToken cancellationToken = default)
    {
        var entity = await _urlRepository.GetByUrlAsync(request.Url, cancellationToken);

        if (entity is not null)
            return entity;

        string token = GenerateRandomAlphanumericString();

        LinkModel model = new()
        {
            Id = 0,
            ShortCode = token,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(1),
            OriginalUrl = request.Url,
        };

        await _urlRepository.AddAsync(model, cancellationToken);

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