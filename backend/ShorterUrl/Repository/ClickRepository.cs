using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;
using ShorterUrl.Models;

namespace ShorterUrl.Repository
{
    public class ClickRepository
    {
        private readonly AppDbContext _context;

        public ClickRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClickModel> AddAsync(ClickModel model, CancellationToken cancellationToken = default)
        {
            await _context.Analytics
                .AddAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<IEnumerable<ClickModel>> AddAsync(IEnumerable<ClickModel> model, CancellationToken cancellationToken = default)
        {
            await _context.Analytics
                .AddRangeAsync(model, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Analytics
                .CountAsync(cancellationToken);
        }

        public async Task<ClickModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Analytics.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Dictionary<string, int>> GetLocations(CancellationToken cancellationToken = default)
        {
            return await _context.Analytics
                .GroupBy(a => a.Location)
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Country, x => x.Count, cancellationToken);
        }

        public async Task<IEnumerable<ClickModel>> GetByLinkIdAsync(int linkId, CancellationToken cancellationToken = default)
        {
            return await _context.Analytics
                .Where(x => x.Link.Id == linkId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> DeleteByLinkIdAsync(int linkId, CancellationToken cancellationToken = default)
        {
            return await _context.Analytics
                .Where(x => x.Link.Id == linkId)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}