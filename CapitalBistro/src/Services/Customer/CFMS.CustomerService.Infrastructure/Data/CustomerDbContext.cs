using Microsoft.EntityFrameworkCore;
using CFMS.CustomerService.Core.Entities;
using System;

namespace CFMS.CustomerService.Infrastructure.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<CustomerInfo> Customers { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
        public DbSet<TierConfig> TierConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.PhoneNumber).IsUnique(); // Ràng buộc SĐT là duy nhất
                
                entity.Property(e => e.TotalAccumulatedPoints).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AvailablePoints).HasColumnType("decimal(18,2)");

                entity.HasMany(c => c.LoyaltyTransactions)
                      .WithOne(lt => lt.Customer)
                      .HasForeignKey(lt => lt.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LoyaltyTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PointsEarned).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PointsRedeemed).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<TierConfig>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MinPointsRequired).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RewardMultiplier).HasColumnType("decimal(18,2)");
            });

            // Seeding Data Hạng Thành Viên
            modelBuilder.Entity<TierConfig>().HasData(
                new TierConfig { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), TierName = "Bronze", MinPointsRequired = 0, RewardMultiplier = 1.0m },
                new TierConfig { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), TierName = "Silver", MinPointsRequired = 1000, RewardMultiplier = 1.2m },
                new TierConfig { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), TierName = "Gold", MinPointsRequired = 5000, RewardMultiplier = 1.5m },
                new TierConfig { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), TierName = "Platinum", MinPointsRequired = 10000, RewardMultiplier = 2.0m }
            );
        }
    }
}
