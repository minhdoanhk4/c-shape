using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.ReportingService.Core.Entities;
using CFMS.ReportingService.Core.Interfaces;
using CFMS.ReportingService.Infrastructure.Data;

namespace CFMS.ReportingService.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportingDbContext _context;

        public ReportRepository(ReportingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DailyRevenueReport>> GetDailyRevenueAsync(Guid? franchiseId, DateOnly startDate, DateOnly endDate)
        {
            var query = _context.DailyRevenueReports.AsQueryable();

            if (franchiseId.HasValue)
            {
                query = query.Where(r => r.FranchiseId == franchiseId.Value);
            }

            return await query
                .Where(r => r.Date >= startDate && r.Date <= endDate)
                .OrderBy(r => r.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<TopSellingProductReport>> GetTopSellingProductsAsync(Guid? franchiseId, DateOnly startDate, DateOnly endDate)
        {
            var query = _context.TopSellingProductReports.AsQueryable();

            if (franchiseId.HasValue)
            {
                query = query.Where(r => r.FranchiseId == franchiseId.Value);
            }

            return await query
                .Where(r => r.Date >= startDate && r.Date <= endDate)
                .OrderByDescending(r => r.QuantitySold)
                .ToListAsync();
        }

        public async Task CreateDailyRevenueReportAsync(DailyRevenueReport report)
        {
            await _context.DailyRevenueReports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task CreateTopSellingProductReportAsync(TopSellingProductReport report)
        {
            await _context.TopSellingProductReports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasAnyDataAsync()
        {
            return await _context.DailyRevenueReports.AnyAsync();
        }
    }
}
