using Microsoft.EntityFrameworkCore;
using CFMS.ProductService.Core.Entities;

namespace CFMS.ProductService.Infrastructure.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Ingredient> Ingredients { get; set; } = null!;
        public DbSet<ProductRecipe> ProductRecipes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
                
                // 1-N Relationship: Category -> Product
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict); // Don't allow deleting category if products exist
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Name).IsRequired().HasMaxLength(150);
                entity.Property(i => i.UnitOfMeasure).IsRequired().HasMaxLength(20);
            });

            modelBuilder.Entity<ProductRecipe>(entity =>
            {
                // Composite primary key for the many-to-many intermediate table
                entity.HasKey(pr => new { pr.ProductId, pr.IngredientId });
                entity.Property(pr => pr.QuantityRequired).HasColumnType("decimal(18,4)");

                entity.HasOne(pr => pr.Product)
                      .WithMany(p => p.ProductRecipes)
                      .HasForeignKey(pr => pr.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(pr => pr.Ingredient)
                      .WithMany(i => i.ProductRecipes)
                      .HasForeignKey(pr => pr.IngredientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
