using ShorterUrl.DTOs;
using ShorterUrl.Exceptions;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service
{
    public class AnalyticsService
    {
        private readonly AnalyticsRepository _analyticsRepository;
        private readonly ShortUrlRepository _shortUrlRepository;

        public AnalyticsService(AnalyticsRepository analyticsRepository, ShortUrlRepository shortUrlRepository)
        {
            _analyticsRepository = analyticsRepository;
            _shortUrlRepository = shortUrlRepository;
        }

        public async Task<AnalyticsDAO> GetById(int id, CancellationToken cancellationToken = default)
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

        private async Task<ShortUrlDAO> GetLinkById(int linkId, CancellationToken cancellationToken = default)
        {
            var link = await _shortUrlRepository.GetByIdAsync(linkId, cancellationToken)
                ?? throw new NotFoundException($"Link with id {linkId} not found.");

            return link;
        }
    }
}