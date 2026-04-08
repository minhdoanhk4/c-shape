using System;
using System.Collections.Generic;

namespace CFMS.CustomerService.Core.Entities
{
    public class CustomerInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        
        public string CurrentTier { get; set; } = "Bronze";
        public decimal TotalAccumulatedPoints { get; set; } = 0;
        public decimal AvailablePoints { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<LoyaltyTransaction> LoyaltyTransactions { get; set; } = new List<LoyaltyTransaction>();
    }
}
