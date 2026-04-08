using System;

namespace CFMS.PromotionService.Core.Entities
{
    public class Coupon
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CampaignId { get; set; }
        public string Code { get; set; } // Unique
        public int MaxUsage { get; set; }
        public int CurrentUsage { get; set; } = 0;
        public Campaign Campaign { get; set; }
    }
}
