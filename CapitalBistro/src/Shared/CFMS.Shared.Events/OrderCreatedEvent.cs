using System;
using System.Collections.Generic;

namespace CFMS.Shared.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid FranchiseId { get; set; }
        public Guid? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderCreatedItem> Items { get; set; } = new List<OrderCreatedItem>();
    }

    public class OrderCreatedItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
