using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using CFMS.PromotionService.Core.Entities;
using CFMS.PromotionService.Core.Interfaces;
using CFMS.PromotionService.API.DTOs;

namespace CFMS.PromotionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionRepository _repository;
        private readonly IDistributedCache _cache;
        private const string CacheKeyPrefix = "promotions_list_";

        public PromotionsController(IPromotionRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        private Guid? GetFranchiseId()
        {
            var fIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(fIdClaim)) return null;
            return Guid.Parse(fIdClaim);
        }

        private bool IsAdmin()
        {
            var roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            return User.Claims.Any(c => c.Type == roleClaimType && c.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet]
        public async Task<IActionResult> GetCampaigns()
        {
            var franchiseId = GetFranchiseId();
            string cacheKey = $"{CacheKeyPrefix}{franchiseId?.ToString() ?? "global"}";

            // Try to get from Cache
            try
            {
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var items = JsonSerializer.Deserialize<List<Campaign>>(cachedData);
                    return Ok(items);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error: {ex.Message}");
            }

            var campaigns = await _repository.GetCampaignsAsync(franchiseId);

            // Store in Cache
            try
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                };
                var jsonData = JsonSerializer.Serialize(campaigns);
                await _cache.SetStringAsync(cacheKey, jsonData, options);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error (Set): {ex.Message}");
            }

            return Ok(campaigns);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest request)
        {
            var userFranchiseId = GetFranchiseId();
            
            // Logic: Manager only creates for their own. Admin can create for any or Global.
            Guid? targetFranchiseId = null;
            if (!IsAdmin())
            {
                targetFranchiseId = userFranchiseId;
            }
            else
            {
                targetFranchiseId = request.FranchiseId; // Admin can specify
            }

            var campaign = new Campaign
            {
                FranchiseId = targetFranchiseId,
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                IsActive = true,
                Coupons = request.Coupons.Select(c => new Coupon
                {
                    Code = c.Code,
                    MaxUsage = c.MaxUsage
                }).ToList()
            };

            await _repository.CreateCampaignAsync(campaign);

            // Invalidate Cache (Specific target OR all if it's admin)
            // For simplicity, we can invalidate both specific and global if it's admin, 
            // or just the specific one.
            await ClearCacheAsync(targetFranchiseId);

            return Created($"/api/promotions/{campaign.Id}", campaign);
        }

        private async Task ClearCacheAsync(Guid? franchiseId)
        {
            try
            {
                string cacheKey = $"{CacheKeyPrefix}{franchiseId?.ToString() ?? "global"}";
                await _cache.RemoveAsync(cacheKey);
                
                // If it's a global campaign, we might want to invalidate others too? 
                // Usually global affects everyone. But for this phase, let's keep it simple.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis Error (Clear): {ex.Message}");
            }
        }

        [HttpPost("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponRequest request)
        {
            var coupon = await _repository.GetCouponByCodeAsync(request.Code);
            if (coupon == null)
            {
                return NotFound("Coupon code not found.");
            }

            var success = await _repository.ApplyCouponAsync(request.Code);
            if (!success)
            {
                return BadRequest("Coupon is invalid, expired, or out of usages.");
            }

            var response = new CouponResponse
            {
                Code = coupon.Code,
                DiscountType = coupon.Campaign.DiscountType,
                DiscountValue = coupon.Campaign.DiscountValue,
                CampaignName = coupon.Campaign.Name,
                IsValid = true
            };

            return Ok(response);
        }
    }
}
