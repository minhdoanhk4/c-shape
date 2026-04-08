using Microsoft.EntityFrameworkCore;
using CFMS.FranchiseService.Core.Entities;

namespace CFMS.FranchiseService.Infrastructure.Data
{
    public class FranchiseDbContext : DbContext
    {
        public FranchiseDbContext(DbContextOptions<FranchiseDbContext> options) : base(options)
        {
        }

        public DbSet<Franchise> Franchises { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Franchise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ContactPhone).HasMaxLength(20);
                
                // Index cho Name để tìm kiếm nhanh
                entity.HasIndex(e => e.Name);
            });
        }
    }
}
