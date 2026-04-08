using System;

namespace CFMS.CustomerService.Core.Entities
{
    public class LoyaltyTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public Guid? FranchiseId { get; set; }
        public Guid? OrderId { get; set; }
        
        public decimal PointsEarned { get; set; }
        public decimal PointsRedeemed { get; set; }
        
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string Note { get; set; }

        public CustomerInfo Customer { get; set; }
    }
}
