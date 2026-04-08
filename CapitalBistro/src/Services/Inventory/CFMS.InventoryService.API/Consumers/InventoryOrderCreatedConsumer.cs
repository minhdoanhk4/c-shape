using System;
using System.Threading.Tasks;
using MassTransit;
using CFMS.Shared.Events;
using CFMS.InventoryService.Core.Interfaces;
using CFMS.InventoryService.Core.Entities;
using Microsoft.Extensions.Logging;

namespace CFMS.InventoryService.API.Consumers
{
    public class InventoryOrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IInventoryRepository _repository;
        private readonly ILogger<InventoryOrderCreatedConsumer> _logger;

        public InventoryOrderCreatedConsumer(IInventoryRepository repository, ILogger<InventoryOrderCreatedConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var @event = context.Message;
            _logger.LogInformation("Processing Inventory for Order: {OrderId}", @event.OrderId);

            try
            {
                foreach (var item in @event.Items)
                {
                    // Giả lập truy xuất Recipe (Trong thực tế sẽ query table Recipes tại InventoryDb)
                    // Ở đây ta đơn giản hóa: 1 Product tiêu tốn 1 đơn vị Ingredient tương ứng (Mock ID)
                    
                    var ingredientId = item.ProductId; // Mock: IngredientId trùng ProductId cho demo
                    
                    var stockItem = await _repository.GetStockItemAsync(@event.FranchiseId, ingredientId);

                    if (stockItem != null)
                    {
                        stockItem.Quantity -= item.Quantity;
                        await _repository.UpdateStockItemAsync(stockItem);

                        // Ghi log giao dịch kho
                        await _repository.AddTransactionAsync(new StockTransaction
                        {
                            StockItemId = stockItem.Id,
                            QuantityChanged = -item.Quantity,
                            TransactionType = CFMS.InventoryService.Core.Enums.TransactionType.Outbound,
                            Reason = "OrderDeduction " + @event.OrderId.ToString(),
                            Timestamp = DateTime.UtcNow
                        });
                        
                        _logger.LogInformation("Deducted {Quantity} from Stock for Ingredient {IngredientId}", item.Quantity, ingredientId);
                    }
                    else
                    {
                        _logger.LogWarning("Stock item not found for Franchise {FranchiseId} and Ingredient {IngredientId}", @event.FranchiseId, ingredientId);
                    }
                }

                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Inventory for Order {OrderId}", @event.OrderId);
                throw; // Re-throw để MassTransit xử lý retry/dead-letter
            }
        }
    }
}
