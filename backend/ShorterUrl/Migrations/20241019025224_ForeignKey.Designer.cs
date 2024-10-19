﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShorterUrl.Data;

#nullable disable

namespace ShorterUrl.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241019025224_ForeignKey")]
    partial class ForeignKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.20");

            modelBuilder.Entity("ShorterUrl.Models.AnalyticsDAO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ClickDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("IpAdress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Referrer")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int?>("ShortUrlDAOId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ShortUrlId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ShortUrlDAOId");

                    b.HasIndex("ShortUrlId");

                    b.ToTable("ClickAnalytics", (string)null);
                });

            modelBuilder.Entity("ShorterUrl.Models.ShortUrlDAO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATE")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("DATE")
                        .HasColumnName("expires_at");

                    b.Property<string>("OriginalUrl")
                        .IsRequired()
                        .HasMaxLength(280)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("original_url");

                    b.Property<string>("ShortCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("short_code");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "ShortCode" }, "IX_URL_TOKEN")
                        .IsUnique();

                    b.ToTable("ShortUrl", (string)null);
                });

            modelBuilder.Entity("ShorterUrl.Models.AnalyticsDAO", b =>
                {
                    b.HasOne("ShorterUrl.Models.ShortUrlDAO", null)
                        .WithMany("Analytics")
                        .HasForeignKey("ShortUrlDAOId");

                    b.HasOne("ShorterUrl.Models.ShortUrlDAO", "ShortUrlDAO")
                        .WithMany()
                        .HasForeignKey("ShortUrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShortUrlDAO");
                });

            modelBuilder.Entity("ShorterUrl.Models.ShortUrlDAO", b =>
                {
                    b.Navigation("Analytics");
                });
#pragma warning restore 612, 618
        }
    }
}
