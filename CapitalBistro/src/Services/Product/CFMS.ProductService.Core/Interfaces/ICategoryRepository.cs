using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.ProductService.Core.Entities;

namespace CFMS.ProductService.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
