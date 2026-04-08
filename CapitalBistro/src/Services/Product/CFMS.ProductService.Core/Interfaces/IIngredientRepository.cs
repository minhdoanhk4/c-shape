using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.ProductService.Core.Entities;

namespace CFMS.ProductService.Core.Interfaces
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<Ingredient?> GetByIdAsync(Guid id);
        Task<Ingredient> AddAsync(Ingredient ingredient);
        Task UpdateAsync(Ingredient ingredient);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
