using System;
using System.Collections.Generic;
using System.Linq;
using CFMS.OrderService.Core.Enums;

namespace CFMS.OrderService.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FranchiseId { get; set; }
        public Guid? CustomerId { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; private set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        private readonly List<OrderItem> _orderItems = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public void AddOrderItem(Guid productId, int quantity, decimal unitPrice)
        {
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
            if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.");

            var item = new OrderItem
            {
                OrderId = this.Id,
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = unitPrice,
                SubTotal = quantity * unitPrice
            };

            _orderItems.Add(item);
            TotalAmount += item.SubTotal;
        }

        public void Validate()
        {
            if (!_orderItems.Any())
            {
                throw new InvalidOperationException("Order must have at least one item.");
            }
        }
    }
}
