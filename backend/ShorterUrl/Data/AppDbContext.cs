using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data.Mappings;
using ShorterUrl.Models;

namespace ShorterUrl.Data;

public class AppDbContext : DbContext
{
    public DbSet<ShortUrlDAO> ShortUrls { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ShortUrlMap());
    }
}
