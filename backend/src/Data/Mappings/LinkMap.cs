using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShorterUrl.Models;

namespace ShorterUrl.Data.Mappings;

public class LinkMap : IEntityTypeConfiguration<LinkModel>
{
    public void Configure(EntityTypeBuilder<LinkModel> builder)
    {
        builder.ToTable("Link");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.Property(x => x.ShortCode)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnType("DATE");

        builder.Property(x => x.ExpiresAt)
            .IsRequired()
            .HasColumnType("DATE");

        builder.Property(x => x.OriginalUrl)
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(280);

        builder
            .HasIndex(x => x.ShortCode, "IX_URL_TOKEN")
            .IsUnique();
    }
}
