using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prva_web_aplikacija.Model;

public partial class PraksaDbContext : DbContext
{
    public PraksaDbContext(DbContextOptions<PraksaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Anime> Animes { get; set; }

    public virtual DbSet<Manga> Mangas { get; set; }

    public virtual DbSet<Studio> Studios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anime>(entity =>
        {
            entity.HasKey(e => e.AnimeId).HasName("anime_pkey");

            entity.ToTable("anime");

            entity.Property(e => e.AnimeId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("anime_id");
            entity.Property(e => e.Genre)
                .HasMaxLength(100)
                .HasColumnName("genre");
            entity.Property(e => e.MangaId).HasColumnName("manga_id");
            entity.Property(e => e.NumberOfSeasons).HasColumnName("number_of_seasons");
            entity.Property(e => e.Popularity)
                .HasMaxLength(255)
                .HasColumnName("popularity");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
            entity.Property(e => e.ReleseDate).HasColumnName("relese_date");
            entity.Property(e => e.StudioId).HasColumnName("studio_id");
            entity.Property(e => e.Title)
                .HasColumnType("character varying")
                .HasColumnName("title");

            entity.HasOne(d => d.Manga).WithMany(p => p.Animes)
                .HasForeignKey(d => d.MangaId)
                .HasConstraintName("anime_manga_id_fkey");

            entity.HasOne(d => d.Studio).WithMany(p => p.Animes)
                .HasForeignKey(d => d.StudioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("anime_studio_id_fkey");
        });

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.MangaId).HasName("manga_pkey");

            entity.ToTable("manga");

            entity.Property(e => e.MangaId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("manga_id");
            entity.Property(e => e.Author)
                .HasMaxLength(255)
                .HasColumnName("author");
            entity.Property(e => e.Popularity)
                .HasMaxLength(255)
                .HasColumnName("popularity");
            entity.Property(e => e.StudioId).HasColumnName("studio_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Volumes).HasColumnName("volumes");

            entity.HasOne(d => d.Studio).WithMany(p => p.Mangas)
                .HasForeignKey(d => d.StudioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("manga_studio_id_fkey");
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasKey(e => e.StudioId).HasName("studio_pkey");

            entity.ToTable("studio");

            entity.HasIndex(e => e.NameS, "studio_name_s_key").IsUnique();

            entity.Property(e => e.StudioId)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("studio_id");
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .HasColumnName("country");
            entity.Property(e => e.Established).HasColumnName("established");
            entity.Property(e => e.NameS)
                .HasMaxLength(255)
                .HasColumnName("name_s");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
