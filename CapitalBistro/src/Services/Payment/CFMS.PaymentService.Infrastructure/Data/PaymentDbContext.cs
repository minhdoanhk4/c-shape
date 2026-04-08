using Microsoft.EntityFrameworkCore;
using CFMS.PaymentService.Core.Entities;

namespace CFMS.PaymentService.Infrastructure.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
                
                // Đánh index để hỗ trợ filter nhanh cho báo cáo Manager và HQ
                entity.HasIndex(e => e.FranchiseId);
                entity.HasIndex(e => e.OrderId);
                entity.HasIndex(e => e.CreatedAt);
            });
        }
    }
}
