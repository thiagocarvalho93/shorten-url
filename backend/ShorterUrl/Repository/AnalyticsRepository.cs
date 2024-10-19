using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;
using ShorterUrl.Models;

namespace ShorterUrl.Repository
{
    public class AnalyticsRepository
    {
        private readonly AppDbContext _context;

        public AnalyticsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AnalyticsDAO> AddAsync(AnalyticsDAO model, CancellationToken cancellationToken = default)
        {
            await _context.Analytics.AddAsync(model);
            await _context.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<AnalyticsDAO?> GetById(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Analytics.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<AnalyticsDAO>> GetByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            return await _context.Analytics
                .Where(x => x.ShortUrlDAO.Id == linkId)
                .ToListAsync(cancellationToken);
        }
    }
}