using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.PromotionService.Core.Entities;
using CFMS.PromotionService.Core.Interfaces;
using CFMS.PromotionService.Infrastructure.Data;

namespace CFMS.PromotionService.Infrastructure.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly PromotionDbContext _context;

        public PromotionRepository(PromotionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Campaign>> GetCampaignsAsync(Guid? franchiseId)
        {
            var query = _context.Campaigns.Include(c => c.Coupons).AsQueryable();

            // Logic: Manager sees their own + Global. Admin sees all? 
            // In microservices, typically we filter by context.
            if (franchiseId.HasValue)
            {
                query = query.Where(c => c.FranchiseId == franchiseId.Value || c.FranchiseId == null);
            }

            return await query.OrderByDescending(c => c.StartDate).ToListAsync();
        }

        public async Task<Campaign> GetCampaignByIdAsync(Guid id)
        {
            return await _context.Campaigns.Include(c => c.Coupons).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateCampaignAsync(Campaign campaign)
        {
            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCampaignAsync(Campaign campaign)
        {
            _context.Campaigns.Update(campaign);
            await _context.SaveChangesAsync();
        }

        public async Task<Coupon> GetCouponByCodeAsync(string code)
        {
            return await _context.Coupons
                .Include(cp => cp.Campaign)
                .FirstOrDefaultAsync(cp => cp.Code == code);
        }

        public async Task<bool> ApplyCouponAsync(string code)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var coupon = await _context.Coupons
                    .Include(cp => cp.Campaign)
                    .FirstOrDefaultAsync(cp => cp.Code == code);

                if (coupon == null) return false;

                var now = DateTime.UtcNow;
                if (!coupon.Campaign.IsActive || 
                    now < coupon.Campaign.StartDate || 
                    now > coupon.Campaign.EndDate)
                {
                    return false;
                }

                if (coupon.CurrentUsage >= coupon.MaxUsage)
                {
                    return false;
                }

                coupon.CurrentUsage++;
                _context.Coupons.Update(coupon);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
