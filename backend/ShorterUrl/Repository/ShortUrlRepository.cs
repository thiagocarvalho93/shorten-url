using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public async Task<List<ShortenUrl>> GetPaginatedAsync(int page, int pageSize)
    {
        return await _context.ShortenUrls
                                .Skip(page * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
    }

    public async Task<ShortenUrl> GetByTokenAsync(string token) => await _context.ShortenUrls.FirstOrDefaultAsync(x => x.Token == token);

    public async Task<ShortenUrl> GetByLongUrlAsync(string longUrl) => await _context.ShortenUrls.FirstOrDefaultAsync(x => x.LongUrl == longUrl);

    public async Task<ShortenUrl> AddAsync(ShortenUrl model)
    {
        await _context.ShortenUrls.AddAsync(model);
        await _context.SaveChangesAsync();

        return model;
    }
}
