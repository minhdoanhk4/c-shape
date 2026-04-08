using Microsoft.EntityFrameworkCore;
using CFMS.InventoryService.Core.Entities;

namespace CFMS.InventoryService.Infrastructure.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MinThreshold).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<StockTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuantityChanged).HasColumnType("decimal(18,2)");

                entity.HasOne(d => d.StockItem)
                    .WithMany()
                    .HasForeignKey(d => d.StockItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
