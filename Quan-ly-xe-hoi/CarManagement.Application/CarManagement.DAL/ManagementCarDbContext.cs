using CarManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CarManagement.DAL;

public partial class ManagementCarDbContext : DbContext
{
    public ManagementCarDbContext()
    {
    }

    public ManagementCarDbContext(DbContextOptions<ManagementCarDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__F267253E0C2784C1");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("accountID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__06B772B9219384C7");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId)
                .ValueGeneratedNever()
                .HasColumnName("brandID");
            entity.Property(e => e.BrandDescription).HasColumnName("brandDescription");
            entity.Property(e => e.BrandName)
                .HasMaxLength(100)
                .HasColumnName("brandName");
            entity.Property(e => e.BrandNational)
                .HasMaxLength(50)
                .HasColumnName("brandNational");
            entity.Property(e => e.FoundingYear).HasColumnName("foundingYear");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("PK__Car__1436F0943C174563");

            entity.ToTable("Car");

            entity.Property(e => e.CarId)
                .ValueGeneratedNever()
                .HasColumnName("carID");
            entity.Property(e => e.BrandId).HasColumnName("brandID");
            entity.Property(e => e.CarDescription).HasColumnName("carDescription");
            entity.Property(e => e.CarEngine)
                .HasMaxLength(50)
                .HasColumnName("carEngine");
            entity.Property(e => e.CarModel)
                .HasMaxLength(100)
                .HasColumnName("carModel");
            entity.Property(e => e.CarSeat).HasColumnName("carSeat");
            entity.Property(e => e.CarWarranty).HasColumnName("carWarranty");
            entity.Property(e => e.DollarPrice).HasColumnName("dollarPrice");
            entity.Property(e => e.TypeId).HasColumnName("typeID");
            entity.Property(e => e.YearPublish).HasColumnName("yearPublish");

            entity.HasOne(d => d.Brand).WithMany(p => p.Cars)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Car_Brand");

            entity.HasOne(d => d.CarType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_Car_Type");
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Type__F04DF11A72B660D5");

            entity.ToTable("CarType");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("typeID");
            entity.Property(e => e.typeModel)
                .HasMaxLength(100)
                .HasColumnName("carType");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TypeDescription).HasColumnName("typeDescription");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("typeName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
