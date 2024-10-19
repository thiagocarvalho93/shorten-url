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

    public async Task<List<ShortUrl>> GetPaginatedAsync(int page, int pageSize)
    {
        return await _context.ShortenUrls
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<ShortUrl> GetByTokenAsync(string token)
    {
        return await _context.ShortenUrls.FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<ShortUrl> GetByUrlAsync(string url)
    {
        return await _context.ShortenUrls.FirstOrDefaultAsync(x => x.Url == url);
    }

    public async Task<ShortUrl> AddAsync(ShortUrl model)
    {
        await _context.ShortenUrls.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }
}
