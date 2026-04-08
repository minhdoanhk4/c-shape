using System;
using System.Collections.Generic;
using CFMS.OrderService.Core.Enums;

namespace CFMS.OrderService.API.DTOs
{
    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class CreatePosOrderRequest
    {
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class CreateOnlineOrderRequest
    {
        public Guid FranchiseId { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
    }

    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}
