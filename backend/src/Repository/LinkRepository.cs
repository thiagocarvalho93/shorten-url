using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;
using ShorterUrl.Helpers;
using ShorterUrl.Models;

namespace ShorterUrl.Repository;

public class LinkRepository
{
    private readonly AppDbContext _context;
    public LinkRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Get
    public async Task<PaginatedResponse<LinkModel>> GetPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await CountAsync(cancellationToken);
        var data = await _context.Links
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<LinkModel>(data, count, page, pageSize);
    }

    public async Task<int[]> GetAllIdsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .Select(x => x.Id)
            .ToArrayAsync(cancellationToken);
    }

    public async Task<LinkModel?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .Include(x => x.Clicks)
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode, cancellationToken);
    }

    public async Task<LinkModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Links
            .Include(x => x.Clicks)
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
    #endregion

    #region Insert
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
    #endregion

    #region Delete
    public async Task<int> DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var count = await _context.Links
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return count;
    }

    public async Task<int> DeleteByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default)
    {
        var count = await _context.Links
            .Where(x => x.ShortCode == shortCode)
            .ExecuteDeleteAsync(cancellationToken);

        return count;
    }

    public async Task<int> DeleteAllAsync(int id, CancellationToken cancellationToken = default)
    {
        var count = await _context.Links
            .ExecuteDeleteAsync(cancellationToken);

        return count;
    }

    #endregion
}
