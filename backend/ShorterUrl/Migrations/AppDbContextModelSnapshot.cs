﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShorterUrl.Data;

#nullable disable

namespace ShorterUrl.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShorterUrl.Models.ShortenUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("DATE")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("DATE")
                        .HasColumnName("expires_at");

                    b.Property<string>("LongUrl")
                        .IsRequired()
                        .HasMaxLength(280)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("long_url");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("VARCHAR")
                        .HasColumnName("token");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Token" }, "IX_URL_TOKEN")
                        .IsUnique();

                    b.ToTable("ShortUrl", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
