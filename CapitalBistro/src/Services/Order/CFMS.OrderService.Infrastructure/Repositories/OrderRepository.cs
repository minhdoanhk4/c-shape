using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.OrderService.Core.Entities;
using CFMS.OrderService.Core.Interfaces;
using CFMS.OrderService.Infrastructure.Data;

namespace CFMS.OrderService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid? franchiseId)
        {
            var query = _context.Orders.Include(o => o.OrderItems).AsQueryable();
            
            if (franchiseId.HasValue)
            {
                query = query.Where(o => o.FranchiseId == franchiseId.Value);
            }

            return await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task CreateOrderAsync(Order order)
        {
            // Tổng tiền hiện đã được tự động tính toán trong Domain Entity (Order.cs) thông qua AddOrderItem
            order.CreatedAt = DateTime.UtcNow;
            
            await _context.Orders.AddAsync(order);
        }

        public Task UpdateOrderAsync(Order order)
        {
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(order);
            return Task.CompletedTask;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
