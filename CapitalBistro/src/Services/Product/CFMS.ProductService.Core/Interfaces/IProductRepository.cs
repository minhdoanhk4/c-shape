using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.ProductService.Core.Entities;

namespace CFMS.ProductService.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllActiveAsync(); // For franchises
        Task<IEnumerable<Product>> GetAllAsync(); // For HQ
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
