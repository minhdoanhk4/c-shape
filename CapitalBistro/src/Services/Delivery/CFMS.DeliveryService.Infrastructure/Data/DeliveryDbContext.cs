using Microsoft.EntityFrameworkCore;
using CFMS.DeliveryService.Core.Entities;

namespace CFMS.DeliveryService.Infrastructure.Data
{
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
        {
        }

        public DbSet<DeliveryJob> DeliveryJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DeliveryJob>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Đánh index để hỗ trợ filter nhanh cho Shipper và Franchise
                entity.HasIndex(e => e.FranchiseId);
                entity.HasIndex(e => e.ShipperId);
                entity.HasIndex(e => e.ReferenceId);
            });
        }
    }
}
