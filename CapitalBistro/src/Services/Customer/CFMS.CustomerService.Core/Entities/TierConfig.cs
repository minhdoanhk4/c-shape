using System;

namespace CFMS.CustomerService.Core.Entities
{
    public class TierConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string TierName { get; set; }
        public decimal MinPointsRequired { get; set; }
        public decimal RewardMultiplier { get; set; }
    }
}
