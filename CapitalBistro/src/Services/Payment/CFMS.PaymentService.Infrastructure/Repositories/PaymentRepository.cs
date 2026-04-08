using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.PaymentService.Core.Entities;
using CFMS.PaymentService.Core.Enums;
using CFMS.PaymentService.Core.Interfaces;
using CFMS.PaymentService.Infrastructure.Data;

namespace CFMS.PaymentService.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentTransaction>> GetTransactionsAsync(Guid? franchiseId, int pageIndex, int pageSize)
        {
            var query = _context.PaymentTransactions.AsQueryable();

            if (franchiseId.HasValue)
            {
                query = query.Where(t => t.FranchiseId == franchiseId.Value);
            }

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<PaymentTransaction> GetTransactionByIdAsync(Guid id)
        {
            return await _context.PaymentTransactions.FindAsync(id);
        }

        public async Task CreateTransactionAsync(PaymentTransaction transaction)
        {
            transaction.CreatedAt = DateTime.UtcNow;
            await _context.PaymentTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionStatusAsync(Guid id, PaymentStatus status, string providerTransactionId = null)
        {
            var transaction = await _context.PaymentTransactions.FindAsync(id);
            if (transaction != null)
            {
                transaction.Status = status;
                transaction.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(providerTransactionId))
                {
                    transaction.ProviderTransactionId = providerTransactionId;
                }

                _context.PaymentTransactions.Update(transaction);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Transaction not found.");
            }
        }
    }
}
