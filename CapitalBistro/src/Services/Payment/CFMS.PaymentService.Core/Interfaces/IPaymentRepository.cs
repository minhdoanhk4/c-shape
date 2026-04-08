using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.PaymentService.Core.Entities;
using CFMS.PaymentService.Core.Enums;

namespace CFMS.PaymentService.Core.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<PaymentTransaction>> GetTransactionsAsync(Guid? franchiseId, int pageIndex, int pageSize);
        Task<PaymentTransaction> GetTransactionByIdAsync(Guid id);
        
        Task CreateTransactionAsync(PaymentTransaction transaction);
        Task UpdateTransactionStatusAsync(Guid id, PaymentStatus status, string providerTransactionId = null);
    }
}
