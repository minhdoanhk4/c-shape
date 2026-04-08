using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.CustomerService.Core.Entities;
using CFMS.CustomerService.Core.Interfaces;
using CFMS.CustomerService.Infrastructure.Data;

namespace CFMS.CustomerService.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerInfo> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers
                .Include(c => c.LoyaltyTransactions)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CustomerInfo> GetCustomerByPhoneAsync(string phone)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == phone);
        }

        public async Task CreateCustomerAsync(CustomerInfo customer)
        {
            customer.CreatedAt = DateTime.UtcNow;
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task AddLoyaltyTransactionAsync(LoyaltyTransaction transaction)
        {
            // Bắt đầu một Transaction mức Database để tránh dính Race Condition
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == transaction.CustomerId);
                if (customer == null) throw new Exception("Customer not found.");

                // 1. Tính toán điểm hiện trường
                customer.AvailablePoints += transaction.PointsEarned;
                customer.AvailablePoints -= transaction.PointsRedeemed;

                if (customer.AvailablePoints < 0)
                {
                    throw new Exception("Not enough points to redeem.");
                }

                customer.TotalAccumulatedPoints += transaction.PointsEarned;

                // 2. Xét hạng (Tier Config Logic)
                var newTier = await _context.TierConfigs
                    .Where(t => t.MinPointsRequired <= customer.TotalAccumulatedPoints)
                    .OrderByDescending(t => t.MinPointsRequired)
                    .FirstOrDefaultAsync();

                if (newTier != null && customer.CurrentTier != newTier.TierName)
                {
                    customer.CurrentTier = newTier.TierName;
                }

                // 3. Gắn dữ liệu và lưu DB
                transaction.TransactionDate = DateTime.UtcNow;
                await _context.LoyaltyTransactions.AddAsync(transaction);
                
                _context.Customers.Update(customer);

                // Commit cả 2 thao tác (Add Log + Update Customer) trong 1 Transaction duy nhất
                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                throw new Exception($"Transaction Failed: {ex.Message}");
            }
        }
    }
}
