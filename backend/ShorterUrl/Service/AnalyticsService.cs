using ShorterUrl.DTOs;
using ShorterUrl.Exceptions;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service
{
    public class AnalyticsService
    {
        private readonly AnalyticsRepository _analyticsRepository;
        private readonly LinkRepository _linkRepository;

        public AnalyticsService(AnalyticsRepository analyticsRepository, LinkRepository linkRepository)
        {
            _analyticsRepository = analyticsRepository;
            _linkRepository = linkRepository;
        }

        public async Task<AnalyticsOverallResponseDTO> GetOverall(CancellationToken cancellationToken = default)
        {
            var clicksCount = await _analyticsRepository.CountAsync(cancellationToken);
            var urlCount = await _linkRepository.CountAsync(cancellationToken);
            var clicksByLocations = await _analyticsRepository.GetLocations(cancellationToken);

            return new AnalyticsOverallResponseDTO
            {
                TotalClicks = clicksCount,
                TotalUrls = urlCount,
                ClicksByLocations = clicksByLocations
            };
        }

        public async Task<AnalyticsModel> GetById(int id, CancellationToken cancellationToken = default)
        {
            var analytics = await _analyticsRepository.GetByIdAsync(id, cancellationToken);

            if (analytics is not null)
                return analytics;

            throw new NotFoundException($"Analytics with id {id} not found.");
        }

        public async Task<AnalyticsResponseDTO> GetByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            var link = await GetLinkById(linkId, cancellationToken);

            var clickList = (await _analyticsRepository.GetByLinkIdAsync(linkId, cancellationToken)).ToList();

            return new AnalyticsResponseDTO
            {
                Clicks = clickList,
                ShortCode = link.ShortCode,
                TotalClicks = clickList.Count,
                CreatedAt = link.CreatedAt,
                ExpiresAt = link.ExpiresAt,
                OriginalUrl = link.OriginalUrl,
            };
        }

        public async Task<int> DeleteByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            await GetLinkById(linkId, cancellationToken);

            return await _analyticsRepository.DeleteByLinkIdAsync(linkId, cancellationToken);
        }

        private async Task<LinkModel> GetLinkById(int linkId, CancellationToken cancellationToken = default)
        {
            var link = await _linkRepository.GetByIdAsync(linkId, cancellationToken)
                ?? throw new NotFoundException($"Link with id {linkId} not found.");

            return link;
        }
    }
}