using AppCar.MinhDA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace AppCar.MinhDA.DAL;

public partial class AppCarManagementDBcontext : DbContext
{
    public AppCarManagementDBcontext()
    {
    }

    public AppCarManagementDBcontext(DbContextOptions<AppCarManagementDBcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    private string GetConnectionString()
    {
        // Sử dụng BaseDirectory để đảm bảo tìm đúng file trong thư mục bin/Debug khi chạy
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .Build();

        // Lấy chuỗi kết nối
        var strConn = config.GetConnectionString("DefaultConnection");

        // Kiểm tra nếu null thì ném lỗi để dễ debug thay vì trả về null gây lỗi ngầm
        if (string.IsNullOrEmpty(strConn))
        {
            throw new Exception("Không tìm thấy chuỗi kết nối 'DefaultConnection' trong file appsettings.json!");
        }

        return strConn;
    }

    // --- HÀM ĐÃ SỬA ---
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Kiểm tra nếu chưa được cấu hình thì mới nạp chuỗi kết nối
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(GetConnectionString());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("graphql", "pg_graphql")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("Account_pkey");

            entity.ToTable("Account", "public");

            entity.Property(e => e.AccountId)
                .ValueGeneratedNever()
                .HasColumnName("accountID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName).HasColumnName("fullName");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.RoleName).HasColumnName("roleName");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("Brand_pkey");

            entity.ToTable("Brand", "public");

            entity.Property(e => e.BrandId)
                .ValueGeneratedNever()
                .HasColumnName("brandID");
            entity.Property(e => e.BrandDescription).HasColumnName("brandDescription");
            entity.Property(e => e.BrandName).HasColumnName("brandName");
            entity.Property(e => e.BrandNational).HasColumnName("brandNational");
            entity.Property(e => e.FoundingYear).HasColumnName("foundingYear");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("Car_pkey");

            entity.ToTable("Car", "public");

            entity.Property(e => e.CarId)
                .ValueGeneratedNever()
                .HasColumnName("carID");
            entity.Property(e => e.BrandId).HasColumnName("brandID");
            entity.Property(e => e.CarDescription).HasColumnName("carDescription");
            entity.Property(e => e.CarEngine)
                .HasMaxLength(50)
                .HasColumnName("carEngine");
            entity.Property(e => e.CarModel).HasColumnName("carModel");
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
                .HasConstraintName("FK_Car_CarType");
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("CarType_pkey");

            entity.ToTable("CarType", "public");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("typeID");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TypeDescription).HasColumnName("typeDescription");
            entity.Property(e => e.TypeModel).HasColumnName("typeModel");
            entity.Property(e => e.TypeName).HasColumnName("typeName");
        });
        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
