using Microsoft.EntityFrameworkCore;
using CFMS.ShiftService.Core.Entities;

namespace CFMS.ShiftService.Infrastructure.Data
{
    public class ShiftDbContext : DbContext
    {
        public ShiftDbContext(DbContextOptions<ShiftDbContext> options) : base(options)
        {
        }

        public DbSet<ShiftConfig> ShiftConfigs { get; set; }
        public DbSet<ShiftAssignment> ShiftAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShiftConfig>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasMany(s => s.Assignments)
                      .WithOne(a => a.ShiftConfig)
                      .HasForeignKey(a => a.ShiftConfigId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ShiftAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.UserId, e.WorkingDate }); // Đánh Index để tối ưu tìm kiếm lịch trùng
            });
        }
    }
}
