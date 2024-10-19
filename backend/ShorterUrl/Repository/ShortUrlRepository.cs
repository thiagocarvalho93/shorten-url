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

    public async Task<List<ShortUrl>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.ShortenUrls
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<ShortUrl?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.ShortenUrls.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task<ShortUrl?> GetByUrlAsync(string url, CancellationToken cancellationToken = default)
    {
        return await _context.ShortenUrls.FirstOrDefaultAsync(x => x.Url == url, cancellationToken);
    }

    public async Task<ShortUrl> AddAsync(ShortUrl model, CancellationToken cancellationToken = default)
    {
        await _context.ShortenUrls.AddAsync(model);
        await _context.SaveChangesAsync(cancellationToken);

        return model;
    }
}
