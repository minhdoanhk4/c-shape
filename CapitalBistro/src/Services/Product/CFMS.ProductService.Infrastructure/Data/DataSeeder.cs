using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CFMS.ProductService.Core.Entities;

namespace CFMS.ProductService.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ProductDbContext>();

            // Ensure migrations are applied
            await context.Database.MigrateAsync();

            // Check if any Categories exist
            if (!await context.Categories.AnyAsync())
            {
                // Create Categories
                var coffeeCategory = new Category { Id = Guid.NewGuid(), Name = "Coffee", Description = "Premium roasted coffee drinks" };
                var teaCategory = new Category { Id = Guid.NewGuid(), Name = "Tea", Description = "Fresh organic teas" };
                var pastryCategory = new Category { Id = Guid.NewGuid(), Name = "Pastry", Description = "Freshly baked pastries" };

                await context.Categories.AddRangeAsync(coffeeCategory, teaCategory, pastryCategory);
                await context.SaveChangesAsync();

                // Create Ingredients
                var coffeeBeans = new Ingredient { Id = Guid.NewGuid(), Name = "Coffee Beans", UnitOfMeasure = "g" };
                var milk = new Ingredient { Id = Guid.NewGuid(), Name = "Fresh Milk", UnitOfMeasure = "ml" };
                var sugar = new Ingredient { Id = Guid.NewGuid(), Name = "Sugar", UnitOfMeasure = "g" };
                var greenTeaLeaf = new Ingredient { Id = Guid.NewGuid(), Name = "Green Tea Leaf", UnitOfMeasure = "g" };
                var water = new Ingredient { Id = Guid.NewGuid(), Name = "Water", UnitOfMeasure = "ml" };

                await context.Ingredients.AddRangeAsync(coffeeBeans, milk, sugar, greenTeaLeaf, water);
                await context.SaveChangesAsync();

                // Create Products
                var espresso = new Product
                {
                    Id = Guid.NewGuid(),
                    CategoryId = coffeeCategory.Id,
                    Name = "Espresso Single Shot",
                    Description = "Strong and bold single shot espresso",
                    Price = 30000,
                    ImageUrl = "espresso.jpg",
                    IsActive = true
                };

                var latte = new Product
                {
                    Id = Guid.NewGuid(),
                    CategoryId = coffeeCategory.Id,
                    Name = "Caffe Latte",
                    Description = "Smooth espresso with steamed milk and a light layer of foam",
                    Price = 55000,
                    ImageUrl = "latte.jpg",
                    IsActive = true
                };

                await context.Products.AddRangeAsync(espresso, latte);
                await context.SaveChangesAsync();

                // Create Product Recipes
                var espressoRecipe1 = new ProductRecipe { ProductId = espresso.Id, IngredientId = coffeeBeans.Id, QuantityRequired = 18 };
                var espressoRecipe2 = new ProductRecipe { ProductId = espresso.Id, IngredientId = water.Id, QuantityRequired = 30 };

                var latteRecipe1 = new ProductRecipe { ProductId = latte.Id, IngredientId = coffeeBeans.Id, QuantityRequired = 18 };
                var latteRecipe2 = new ProductRecipe { ProductId = latte.Id, IngredientId = milk.Id, QuantityRequired = 150 };

                await context.ProductRecipes.AddRangeAsync(espressoRecipe1, espressoRecipe2, latteRecipe1, latteRecipe2);
                await context.SaveChangesAsync();
            }
        }
    }
}
