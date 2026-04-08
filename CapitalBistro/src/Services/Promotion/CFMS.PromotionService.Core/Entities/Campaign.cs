using System;
using System.Collections.Generic;
using CFMS.PromotionService.Core.Enums;

namespace CFMS.PromotionService.Core.Entities
{
    public class Campaign
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? FranchiseId { get; set; } // null means Global
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    }
}
