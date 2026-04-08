using System;
using CFMS.PaymentService.Core.Enums;

namespace CFMS.PaymentService.Core.Entities
{
    public class PaymentTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid FranchiseId { get; set; }
        
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Lưu vết Mã hoá đơn bên thứ 3 (VD: vnp_TransactionNo)
        public string ProviderTransactionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
