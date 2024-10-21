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
                .ToDictionaryAsync(x => ReturnStringOrUnknown(x.Country), x => x.Count, cancellationToken);
        }

        public async Task<Dictionary<string, int>> GetLocationsByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            return await _context.Analytics
                .Where(x => x.LinkId == linkId)
                .GroupBy(a => a.Location)
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => ReturnStringOrUnknown(x.Country), x => x.Count, cancellationToken);
        }

        public async Task<Dictionary<string, int>> GetDeviceLanguagesByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            return await _context.Analytics
                .Where(x => x.LinkId == linkId)
                .GroupBy(a => a.DeviceLanguage)
                .Select(g => new { DeviceLanguage = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => ReturnStringOrUnknown(x.DeviceLanguage), x => x.Count, cancellationToken);
        }

        public async Task<Dictionary<string, int>> GetDaysOfWeekByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            var analytics = await _context.Analytics
                .Where(x => x.LinkId == linkId)
                .GroupBy(a => a.ClickDate.DayOfWeek)
                .Select(g => new { DayOfWeek = g.Key.ToString(), Count = g.Count() })
                .ToListAsync(cancellationToken);

            return analytics.ToDictionary(x => ReturnStringOrUnknown(x.DayOfWeek), x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetDayHoursByLinkId(int linkId, CancellationToken cancellationToken = default)
        {
            var analytics = await _context.Analytics
                .Where(x => x.LinkId == linkId)
                .GroupBy(a => a.ClickDate.Hour)
                .Select(g => new { Hour = g.Key.ToString(), Count = g.Count() })
                .ToListAsync(cancellationToken);

            return analytics.ToDictionary(x => ReturnStringOrUnknown(x.Hour), x => x.Count);
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

        private static string ReturnStringOrUnknown(string? str)
        {
            return String.IsNullOrEmpty(str) ? "unknown" : str;
        }
    }
}