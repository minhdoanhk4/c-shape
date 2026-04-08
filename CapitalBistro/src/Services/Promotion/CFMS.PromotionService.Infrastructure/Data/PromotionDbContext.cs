using Microsoft.EntityFrameworkCore;
using CFMS.PromotionService.Core.Entities;

namespace CFMS.PromotionService.Infrastructure.Data
{
    public class PromotionDbContext : DbContext
    {
        public PromotionDbContext(DbContextOptions<PromotionDbContext> options) : base(options)
        {
        }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DiscountValue).HasColumnType("decimal(18,2)");
                
                entity.HasMany(c => c.Coupons)
                      .WithOne(cp => cp.Campaign)
                      .HasForeignKey(cp => cp.CampaignId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Code).IsUnique(); // UNIQUE Coupon Code
            });
        }
    }
}
