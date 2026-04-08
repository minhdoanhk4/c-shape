using System;
using CFMS.InventoryService.Core.Enums;

namespace CFMS.InventoryService.Core.Entities
{
    public class StockTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StockItemId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal QuantityChanged { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Reason { get; set; }

        public StockItem StockItem { get; set; }
    }
}
