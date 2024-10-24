using ShorterUrl.DTOs;
using ShorterUrl.Exceptions;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service
{
    public class AnalyticsService
    {
        private readonly ClickRepository _clickRepository;
        private readonly LinkRepository _linkRepository;

        public AnalyticsService(ClickRepository analyticsRepository, LinkRepository linkRepository)
        {
            _clickRepository = analyticsRepository;
            _linkRepository = linkRepository;
        }

        public async Task<GeneralAnalyticsResponseDTO> GetGeneralAnalytics(CancellationToken cancellationToken = default)
        {
            var clicksCount = await _clickRepository.CountAsync(cancellationToken);
            var urlCount = await _linkRepository.CountAsync(cancellationToken);
            var clicksByLocations = await _clickRepository.GetLocations(cancellationToken);

            return new GeneralAnalyticsResponseDTO
            {
                TotalClicks = clicksCount,
                TotalUrls = urlCount,
                ClicksByLocations = clicksByLocations
            };
        }

        public async Task<ClickModel> GetById(int id, CancellationToken cancellationToken = default)
        {
            var analytics = await _clickRepository.GetByIdAsync(id, cancellationToken);

            if (analytics is not null)
                return analytics;

            throw new NotFoundException($"Analytics with id {id} not found.");
        }

        public async Task<LinkAnalyticsResponseDTO> GetByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkById(linkId, cancellationToken);

            var clickList = (await _clickRepository.GetByLinkIdAsync(linkId, cancellationToken)).ToList();

            return new LinkAnalyticsResponseDTO
            {
                Clicks = clickList,
                ShortCode = link.ShortCode,
                TotalClicks = clickList.Count,
                CreatedAt = link.CreatedAt,
                ExpiresAt = link.ExpiresAt,
                OriginalUrl = link.OriginalUrl,
            };
        }

        public async Task<LinkAnalyticsResponseDTO> GetByLinkShortUrl(string shortCode, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkByShortCode(shortCode, cancellationToken);

            var clickList = (await _clickRepository.GetByLinkIdAsync(link.Id, cancellationToken)).ToList();

            return new LinkAnalyticsResponseDTO
            {
                Clicks = clickList,
                ShortCode = link.ShortCode,
                TotalClicks = clickList.Count,
                CreatedAt = link.CreatedAt,
                ExpiresAt = link.ExpiresAt,
                OriginalUrl = link.OriginalUrl,
            };
        }

        public async Task<Dictionary<string, int>> GetClickLocationsByLinkShortUrl(string shortCode, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkByShortCode(shortCode, cancellationToken);

            var locations = await _clickRepository.GetLocationsByLinkId(link.Id, cancellationToken);

            return locations;
        }

        public async Task<Dictionary<string, int>> GetClickDeviceLanguageByLinkShortUrl(string shortCode, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkByShortCode(shortCode, cancellationToken);

            var locations = await _clickRepository.GetDeviceLanguagesByLinkId(link.Id, cancellationToken);

            return locations;
        }

        public async Task<Dictionary<string, int>> GetClickDaysOfWeekByLinkShortUrl(string shortCode, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkByShortCode(shortCode, cancellationToken);

            var locations = await _clickRepository.GetDaysOfWeekByLinkId(link.Id, cancellationToken);

            return locations;
        }

        public async Task<Dictionary<string, int>> GetClickDayHoursByLinkShortUrl(string shortCode, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkByShortCode(shortCode, cancellationToken);

            var locations = await _clickRepository.GetDayHoursByLinkId(link.Id, cancellationToken);

            return locations;
        }

        public async Task<int> DeleteByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            await GetLinkById(linkId, cancellationToken);

            return await _clickRepository.DeleteByLinkIdAsync(linkId, cancellationToken);
        }

        public async Task<int> DeleteByLinkShortCode(string shortCode, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkByShortCode(shortCode, cancellationToken);

            return await _clickRepository.DeleteByLinkIdAsync(link.Id, cancellationToken);
        }

        private async Task<LinkModel> GetLinkById(int linkId, CancellationToken cancellationToken = default)
        {
            var link = await _linkRepository.GetByIdAsync(linkId, cancellationToken)
                ?? throw new NotFoundException($"Link with id {linkId} not found.");

            return link;
        }

        private async Task<LinkModel> GetLinkByShortCode(string shortCode, CancellationToken cancellationToken)
        {
            var link = await _linkRepository.GetByShortCodeAsync(shortCode, cancellationToken)
                ?? throw new NotFoundException($"Link with short code {shortCode} not found.");

            return link;
        }
    }
}