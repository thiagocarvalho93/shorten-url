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

    public async Task<ShortUrlDAO?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<ShortUrlDAO?> GetByUrlAsync(string? url, CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(x => x.Url == url, cancellationToken);
    }

    public async Task<ShortUrlDAO> AddAsync(ShortUrlDAO model, CancellationToken cancellationToken = default)
    {
        await _context.ShortUrls.AddAsync(model);
        await _context.SaveChangesAsync(cancellationToken);

        return model;
    }
}
