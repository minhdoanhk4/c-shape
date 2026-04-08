using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CFMS.InventoryService.Core.Entities;
using CFMS.InventoryService.Core.Interfaces;
using CFMS.InventoryService.API.DTOs;

namespace CFMS.InventoryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _repository;

        public InventoryController(IInventoryRepository repository)
        {
            _repository = repository;
        }

        private Guid? GetFranchiseId()
        {
            var fIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(fIdClaim))
            {
                return null; // Admin HQ
            }
            return Guid.Parse(fIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetInventory()
        {
            var franchiseId = GetFranchiseId();
            var items = await _repository.GetStockItemsAsync(franchiseId);
            return Ok(items);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var franchiseId = GetFranchiseId();
            var items = await _repository.GetLowStockItemsAsync(franchiseId);
            return Ok(items);
        }

        [HttpPost("transaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] StockTransactionRequest request)
        {
            var franchiseId = GetFranchiseId();
            if (franchiseId == null)
            {
                return Forbid("Only Franchise staff/managers can execute stock transactions.");
            }

            var stockItem = await _repository.GetStockItemAsync(franchiseId.Value, request.IngredientId);

            if (stockItem == null)
            {
                // Tự động tạo StockItem nếu chưa có trong kho
                stockItem = new StockItem
                {
                    FranchiseId = franchiseId.Value,
                    IngredientId = request.IngredientId,
                    Quantity = 0, // Bắt đầu bằng 0, lượng changed sẽ được cộng vào sau
                    Unit = "units", // Default unit (Có thể fetch từ Product service sau nếu cần thiết)
                    MinThreshold = 10 
                };
                await _repository.CreateStockItemAsync(stockItem);
                
                // Mặc định DB Context theo dõi và gán ID mới. Lưu nháp trước khi Add Transaction.
                await _repository.SaveChangesAsync(); 
            }

            var transaction = new StockTransaction
            {
                StockItemId = stockItem.Id,
                TransactionType = request.TransactionType,
                QuantityChanged = request.QuantityChanged,
                Reason = request.Reason ?? string.Empty
            };

            await _repository.AddTransactionAsync(transaction);

            if (await _repository.SaveChangesAsync())
            {
                return Ok(new { Message = "Transaction created successfully. Stock updated." });
            }

            return BadRequest("Failed to process transaction.");
        }
    }
}
