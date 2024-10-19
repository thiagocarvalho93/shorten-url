using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;
using ShorterUrl.Models;

namespace ShorterUrl.Repository;

public class LinkRepository
{
    private readonly AppDbContext _context;
    public LinkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LinkModel>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int[]> GetAllIdsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .Select(x => x.Id)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<LinkModel?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .Include(x => x.Analytics)
            .FirstOrDefaultAsync(x => x.ShortCode == token, cancellationToken);
    }

    public async Task<LinkModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .Include(x => x.Analytics)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<LinkModel?> GetByUrlAsync(string? url, CancellationToken cancellationToken = default)
    {
        return await _context.Links.FirstOrDefaultAsync(x => x.OriginalUrl == url, cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .CountAsync(cancellationToken);
    }

    public async Task<LinkModel> AddAsync(LinkModel model, CancellationToken cancellationToken = default)
    {
        await _context.Links.AddAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return model;
    }

    public async Task<IEnumerable<LinkModel>> AddAsync(IEnumerable<LinkModel> model, CancellationToken cancellationToken = default)
    {
        await _context.Links.AddRangeAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return model;
    }
}
