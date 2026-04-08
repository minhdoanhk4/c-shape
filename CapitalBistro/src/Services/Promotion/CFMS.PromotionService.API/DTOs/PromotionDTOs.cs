using System;
using System.Collections.Generic;
using CFMS.PromotionService.Core.Enums;

namespace CFMS.PromotionService.API.DTOs
{
    public class CreateCampaignRequest
    {
        public Guid? FranchiseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public List<CreateCouponRequest> Coupons { get; set; } = new List<CreateCouponRequest>();
    }

    public class CreateCouponRequest
    {
        public string Code { get; set; }
        public int MaxUsage { get; set; }
    }

    public class ApplyCouponRequest
    {
        public string Code { get; set; }
    }

    public class CouponResponse
    {
        public string Code { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public string CampaignName { get; set; }
        public bool IsValid { get; set; }
    }
}
