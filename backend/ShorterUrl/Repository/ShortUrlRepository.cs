using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;
using ShorterUrl.Models;

namespace ShorterUrl.Repository;

public class ShortUrlRepository
{
    private readonly AppDbContext _context;
    public ShortUrlRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ShortUrlDAO>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int[]> GetAllIdsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls
            .Select(x => x.Id)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<ShortUrlDAO?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls
            .Include(x => x.Analytics)
            .FirstOrDefaultAsync(x => x.ShortCode == token, cancellationToken);
    }

    public async Task<ShortUrlDAO?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls
            .Include(x => x.Analytics)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ShortUrlDAO?> GetByUrlAsync(string? url, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(x => x.OriginalUrl == url, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls
            .CountAsync(cancellationToken);
    }

    public async Task<ShortUrlDAO> AddAsync(ShortUrlDAO model, CancellationToken cancellationToken = default)
    {
        await _context.ShortUrls.AddAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return model;
    }

    public async Task<IEnumerable<ShortUrlDAO>> AddAsync(IEnumerable<ShortUrlDAO> model, CancellationToken cancellationToken = default)
    {
        await _context.ShortUrls.AddRangeAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return model;
    }
}
