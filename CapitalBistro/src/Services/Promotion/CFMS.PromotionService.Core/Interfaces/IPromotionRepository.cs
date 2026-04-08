using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.PromotionService.Core.Entities;

namespace CFMS.PromotionService.Core.Interfaces
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Campaign>> GetCampaignsAsync(Guid? franchiseId);
        Task<Campaign> GetCampaignByIdAsync(Guid id);
        Task CreateCampaignAsync(Campaign campaign);
        Task UpdateCampaignAsync(Campaign campaign);
        
        Task<Coupon> GetCouponByCodeAsync(string code);
        Task<bool> ApplyCouponAsync(string code);
        Task<bool> SaveChangesAsync();
    }
}
