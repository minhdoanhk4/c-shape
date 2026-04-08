using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.OrderService.Core.Entities;

namespace CFMS.OrderService.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Guid? franchiseId);
        Task<Order> GetOrderByIdAsync(Guid id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task<bool> SaveChangesAsync();
    }
}
