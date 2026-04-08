using System;

namespace CFMS.InventoryService.Core.Entities
{
    public class StockItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FranchiseId { get; set; }
        public Guid IngredientId { get; set; }
        public decimal Quantity { get; set; }
        public required string Unit { get; set; }
        public decimal MinThreshold { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
