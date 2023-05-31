using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShorterUrl.Models;

namespace ShorterUrl.Data.Mappings;

public class ShortenUrlMap : IEntityTypeConfiguration<ShortenUrl>
{
    public void Configure(EntityTypeBuilder<ShortenUrl> builder)
    {
        // Tabela
        builder.ToTable("ShortUrl");

        // Chave Primária
        builder.HasKey(x => x.Id);

        // Identity
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        // Propriedades
        builder.Property(x => x.Token)
            .IsRequired()
            .HasColumnName("token")
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

        // Propriedades
        builder.Property(x => x.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at")
            .HasColumnType("DATE");

        // Propriedades
        builder.Property(x => x.ExpiresAt)
            .IsRequired()
            .HasColumnName("expires_at")
            .HasColumnType("DATE");

        // Propriedades
        builder.Property(x => x.LongUrl)
            .IsRequired()
            .HasColumnName("long_url")
            .HasColumnType("VARCHAR")
            .HasMaxLength(280);

        // Índices
        builder
            .HasIndex(x => x.Token, "IX_URL_TOKEN")
            .IsUnique();
    }
}
