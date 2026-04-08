using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.InventoryService.Core.Entities;

namespace CFMS.InventoryService.Core.Interfaces
{
    public interface IInventoryRepository
    {
        Task<IEnumerable<StockItem>> GetStockItemsAsync(Guid? franchiseId);
        Task<IEnumerable<StockItem>> GetLowStockItemsAsync(Guid? franchiseId);
        Task<StockItem> GetStockItemAsync(Guid franchiseId, Guid ingredientId);
        Task CreateStockItemAsync(StockItem item);
        Task UpdateStockItemAsync(StockItem item);
        Task AddTransactionAsync(StockTransaction transaction);
        Task<bool> SaveChangesAsync();
    }
}
