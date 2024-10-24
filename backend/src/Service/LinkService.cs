using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using ShorterUrl.DTOs;
using ShorterUrl.Exceptions;
using ShorterUrl.Helpers;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service;

public class LinkService
{
    public readonly LinkRepository _linkRepository;
    public readonly ClickRepository _clickRepository;
    private readonly LocationService _locationService;
    private readonly IMemoryCache _cache;

    public LinkService(LinkRepository linkRepository, ClickRepository clickRepository, IMemoryCache cache, LocationService locationService)
    {
        _linkRepository = linkRepository;
        _clickRepository = clickRepository;
        _cache = cache;
        _locationService = locationService;
    }

    public async Task<PaginatedResponse<LinkModel>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _linkRepository.GetPaginatedAsync(page, pageSize, cancellationToken);
    }

    public async Task<PaginatedResponse<LinkModel>> GetPaginatedByUserIdAsync(int page, int pageSize, int userId, CancellationToken cancellationToken = default)
    {
        return await _linkRepository.GetPaginatedByUserIdAsync(page, pageSize, userId, cancellationToken);
    }

    public async Task<LinkModel> RedirectByShortCodeAsync(string shortCode, ClickRequestDTO? clickRequest, CancellationToken cancellationToken = default)
    {
        var link = await GetByShortCodeAsync(shortCode, cancellationToken);

        await AddClick(clickRequest, link, cancellationToken);

        return link;
    }

    public async Task<LinkModel> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        var link = await _cache.GetOrCreateAsync(shortCode, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2);
            return await _linkRepository.GetByShortCodeAsync(shortCode, cancellationToken);
        });

        if (link is not null)
            return link;

        throw new NotFoundException($"Short code {shortCode} not found.");
    }

    public async Task<LinkModel> InsertAsync(LinkInsertRequestDTO request, string userId, CancellationToken cancellationToken = default)
    {
        var entity = await _linkRepository.GetByUrlAsync(request.Url, cancellationToken);

        if (entity is not null)
            return entity;

        string shortCode = GenerateRandomAlphanumericString(7);

        LinkModel model = new()
        {
            Id = 0,
            ShortCode = shortCode,
            CreatedAt = DateTime.Now,
            ExpiresAt = DateTime.Now.AddDays(1),
            OriginalUrl = request.Url,
            UserId = Int32.Parse(userId)
        };

        await _linkRepository.AddAsync(model, cancellationToken);

        return model;
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleteCount = await _linkRepository.DeleteByIdAsync(id, cancellationToken);

        if (deleteCount == 0)
            throw new NotFoundException($"Link with id {id} not found.");
    }

    public async Task DeleteByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        var deleteCount = await _linkRepository.DeleteByShortCodeAsync(shortCode, cancellationToken);

        if (deleteCount == 0)
            throw new NotFoundException($"Link with short code {shortCode} not found.");
    }

    public async Task ChangeAlias(string shortCode, string newAlias, CancellationToken cancellationToken = default)
    {
        var link = await GetByShortCodeAsync(shortCode, cancellationToken);

        await _linkRepository.ChangeAliasAsync(shortCode, newAlias, cancellationToken);
    }

    private async Task AddClick(ClickRequestDTO clickRequest, LinkModel link, CancellationToken cancellationToken)
    {
        var location = await GetLocationSafelyAsync(clickRequest.IpAdress);

        var click = new ClickModel()
        {
            LinkId = link.Id,
            ClickDate = DateTime.Now,
            IpAdress = clickRequest?.IpAdress ?? "",
            Location = location,
            Referrer = clickRequest?.Referrer ?? "",
            UserAgent = clickRequest?.UserAgent ?? "",
            DeviceLanguage = clickRequest?.DeviceLanguage ?? "",
        };
        await _clickRepository.AddAsync(click, cancellationToken);
    }

    private async Task<string> GetLocationSafelyAsync(string ipAddress)
    {
        try
        {
            return await _locationService.GetLocationAsync(ipAddress);
        }
        catch
        {
            return string.Empty;
        }
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