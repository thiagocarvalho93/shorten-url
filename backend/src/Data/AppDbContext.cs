using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data.Mappings;
using ShorterUrl.Models;

namespace ShorterUrl.Data;

public class AppDbContext : DbContext
{
    public DbSet<LinkModel> Links { get; set; }
    public DbSet<ClickModel> Analytics { get; set; }
    public DbSet<UserModel> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LinkMap());
        modelBuilder.ApplyConfiguration(new ClickMap());
        modelBuilder.ApplyConfiguration(new UserMap());
    }
}
