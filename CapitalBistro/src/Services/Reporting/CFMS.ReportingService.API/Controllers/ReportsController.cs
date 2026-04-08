using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.ReportingService.Core.Entities;
using CFMS.ReportingService.Core.Interfaces;
using CFMS.ReportingService.API.DTOs;

namespace CFMS.ReportingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _repository;

        public ReportsController(IReportRepository repository)
        {
            _repository = repository;
        }

        private Guid? GetFranchiseIdFromToken()
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

        [HttpGet("daily-revenue")]
        public async Task<IActionResult> GetDailyRevenue([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] Guid? filterFranchiseId = null)
        {
            try
            {
                var start = DateOnly.Parse(startDate);
                var end = DateOnly.Parse(endDate);
                
                Guid? targetFranchiseId = null;

                if (!IsAdmin())
                {
                    // Manager chỉ được xem của chính mình
                    targetFranchiseId = GetFranchiseIdFromToken();
                    if (targetFranchiseId == null) return Forbid();
                }
                else
                {
                    // Admin có thể xem tất cả (null) hoặc xem theo filterFranchiseId truyền vào
                    targetFranchiseId = filterFranchiseId;
                }

                var reports = await _repository.GetDailyRevenueAsync(targetFranchiseId, start, end);
                
                var response = reports.Select(r => new RevenueReportResponse
                {
                    FranchiseId = r.FranchiseId,
                    Date = r.Date,
                    TotalRevenue = r.TotalRevenue,
                    TotalOrders = r.TotalOrders
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] Guid? filterFranchiseId = null)
        {
            try
            {
                var start = DateOnly.Parse(startDate);
                var end = DateOnly.Parse(endDate);

                Guid? targetFranchiseId = null;

                if (!IsAdmin())
                {
                    targetFranchiseId = GetFranchiseIdFromToken();
                    if (targetFranchiseId == null) return Forbid();
                }
                else
                {
                    targetFranchiseId = filterFranchiseId;
                }

                var reports = await _repository.GetTopSellingProductsAsync(targetFranchiseId, start, end);

                var response = reports.Select(r => new TopProductResponse
                {
                    FranchiseId = r.FranchiseId,
                    Date = r.Date,
                    ProductId = r.ProductId,
                    ProductName = r.ProductName,
                    QuantitySold = r.QuantitySold
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
