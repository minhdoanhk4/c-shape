using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.ReportingService.Core.Entities;

namespace CFMS.ReportingService.Core.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<DailyRevenueReport>> GetDailyRevenueAsync(Guid? franchiseId, DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<TopSellingProductReport>> GetTopSellingProductsAsync(Guid? franchiseId, DateOnly startDate, DateOnly endDate);
        Task CreateDailyRevenueReportAsync(DailyRevenueReport report);
        Task CreateTopSellingProductReportAsync(TopSellingProductReport report);
        Task<bool> HasAnyDataAsync();
    }
}
