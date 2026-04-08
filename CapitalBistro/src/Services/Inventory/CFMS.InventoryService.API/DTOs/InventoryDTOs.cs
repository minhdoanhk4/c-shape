using System;
using CFMS.InventoryService.Core.Enums;

namespace CFMS.InventoryService.API.DTOs
{
    public class StockTransactionRequest
    {
        public Guid IngredientId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal QuantityChanged { get; set; }
        public string? Reason { get; set; }
    }
}
