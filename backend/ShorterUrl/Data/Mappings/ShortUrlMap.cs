using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShorterUrl.Models;

namespace ShorterUrl.Data.Mappings;

public class ShortUrlMap : IEntityTypeConfiguration<ShortUrlDAO>
{
    public void Configure(EntityTypeBuilder<ShortUrlDAO> builder)
    {
        builder.ToTable("ShortUrl");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.Token)
            .IsRequired()
            .HasColumnName("token")
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasColumnType("DATE");

        builder.Property(x => x.ExpiresAt)
            .IsRequired()
            .HasColumnName("expires_at")
            .HasColumnType("DATE");

        builder.Property(x => x.Url)
            .IsRequired()
            .HasColumnName("long_url")
            .HasColumnType("VARCHAR")
            .HasMaxLength(280);

        builder
            .HasIndex(x => x.Token, "IX_URL_TOKEN")
            .IsUnique();
    }
}
