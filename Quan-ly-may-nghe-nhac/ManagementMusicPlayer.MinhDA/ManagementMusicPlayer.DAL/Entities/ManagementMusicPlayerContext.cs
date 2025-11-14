using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ManagementMusicPlayer.DAL.Entities;

public partial class ManagementMusicPlayerContext : DbContext
{
    public ManagementMusicPlayerContext()
    {
    }

    public ManagementMusicPlayerContext(DbContextOptions<ManagementMusicPlayerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<MusicPlayer> MusicPlayers { get; set; }


    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F9503FA04");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E6164B154D227").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1F834CD17C9");

            entity.ToTable("Category");

            entity.HasIndex(e => e.CategoryName, "UQ__Category__37077ABDC078BE1B").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Company__AD5459B02E6960A3");

            entity.ToTable("Company");

            entity.HasIndex(e => e.CompanyName, "UQ__Company__B31074169FC24E93").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("companyID");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .HasColumnName("companyName");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");
        });

        modelBuilder.Entity<MusicPlayer>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK__MusicPla__2CDA01D173A53B13");

            entity.ToTable("MusicPlayer");

            entity.Property(e => e.PlayerId).HasColumnName("playerID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CompanyId).HasColumnName("companyID");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(100)
                .HasColumnName("playerName");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.PublishDate).HasColumnName("publishDate");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Category).WithMany(p => p.MusicPlayers)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MusicPlay__categ__2F10007B");

            entity.HasOne(d => d.Company).WithMany(p => p.MusicPlayers)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MusicPlay__compa__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
