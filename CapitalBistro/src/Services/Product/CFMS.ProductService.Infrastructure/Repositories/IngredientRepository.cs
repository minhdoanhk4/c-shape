using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.ProductService.Core.Entities;
using CFMS.ProductService.Core.Interfaces;
using CFMS.ProductService.Infrastructure.Data;

namespace CFMS.ProductService.Infrastructure.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly ProductDbContext _context;

        public IngredientRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<Ingredient?> GetByIdAsync(Guid id)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Ingredient> AddAsync(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
            return ingredient;
        }

        public Task UpdateAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var ingredient = await GetByIdAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
