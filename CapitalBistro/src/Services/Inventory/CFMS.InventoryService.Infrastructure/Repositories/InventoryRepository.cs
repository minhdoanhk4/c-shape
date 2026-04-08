using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.InventoryService.Core.Entities;
using CFMS.InventoryService.Core.Interfaces;
using CFMS.InventoryService.Infrastructure.Data;

namespace CFMS.InventoryService.Infrastructure.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly InventoryDbContext _context;

        public InventoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockItem>> GetStockItemsAsync(Guid? franchiseId)
        {
            var query = _context.StockItems.AsQueryable();
            if (franchiseId.HasValue)
            {
                query = query.Where(x => x.FranchiseId == franchiseId.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<StockItem>> GetLowStockItemsAsync(Guid? franchiseId)
        {
            var query = _context.StockItems.Where(x => x.Quantity <= x.MinThreshold);
            if (franchiseId.HasValue)
            {
                query = query.Where(x => x.FranchiseId == franchiseId.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<StockItem> GetStockItemAsync(Guid franchiseId, Guid ingredientId)
        {
            return await _context.StockItems
                .FirstOrDefaultAsync(x => x.FranchiseId == franchiseId && x.IngredientId == ingredientId);
        }

        public async Task CreateStockItemAsync(StockItem item)
        {
            await _context.StockItems.AddAsync(item);
        }

        public Task UpdateStockItemAsync(StockItem item)
        {
            _context.StockItems.Update(item);
            return Task.CompletedTask;
        }

        public async Task AddTransactionAsync(StockTransaction transaction)
        {
            // Lấy StockItem ra để thực hiện trừ/cộng dồn
            var stockItem = await _context.StockItems.FindAsync(transaction.StockItemId);
            if (stockItem != null)
            {
                stockItem.Quantity += transaction.QuantityChanged;
                stockItem.LastUpdated = DateTime.UtcNow;
                _context.StockItems.Update(stockItem);
            }

            await _context.StockTransactions.AddAsync(transaction);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // Vì DbContext default hỗ trợ 1 transaction per SaveChangesAsync() 
            // nên toàn vẹn dữ liệu cho AddTransaction được đảm bảo
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
