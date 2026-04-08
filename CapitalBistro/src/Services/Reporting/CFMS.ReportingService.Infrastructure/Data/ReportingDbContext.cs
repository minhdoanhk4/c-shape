using Microsoft.EntityFrameworkCore;
using CFMS.ReportingService.Core.Entities;

namespace CFMS.ReportingService.Infrastructure.Data
{
    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options)
        {
        }

        public DbSet<DailyRevenueReport> DailyRevenueReports { get; set; }
        public DbSet<TopSellingProductReport> TopSellingProductReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DailyRevenueReport>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalRevenue).HasColumnType("decimal(18,2)");
                entity.HasIndex(e => new { e.FranchiseId, e.Date });
            });

            modelBuilder.Entity<TopSellingProductReport>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.FranchiseId, e.Date });
            });
        }
    }
}
