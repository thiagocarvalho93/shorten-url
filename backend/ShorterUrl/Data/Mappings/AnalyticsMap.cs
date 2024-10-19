using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShorterUrl.Models;

namespace ShorterUrl.Data.Mappings;

public class AnalyticsMap : IEntityTypeConfiguration<AnalyticsDAO>
{
    public void Configure(EntityTypeBuilder<AnalyticsDAO> builder)
    {
        builder.ToTable("ClickAnalytics");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.IpAdress)
            .HasMaxLength(45)
            .IsRequired(false);

        builder.Property(a => a.UserAgent)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(a => a.Location)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(a => a.Referrer)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(a => a.ClickDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(a => a.ShortUrlDAO)
            .WithMany()
            .HasForeignKey("ShortUrlId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}