using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.ReportingService.Core.Entities;
using CFMS.ReportingService.Core.Interfaces;

namespace CFMS.ReportingService.Infrastructure.Data
{
    public class ReportingDataSeeder
    {
        public static async Task SeedAsync(IReportRepository repository)
        {
            if (await repository.HasAnyDataAsync()) return;

            var franchiseA = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var franchiseB = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // Seed Daily Revenue
            for (int i = 0; i < 10; i++)
            {
                var date = today.AddDays(-i);
                
                await repository.CreateDailyRevenueReportAsync(new DailyRevenueReport
                {
                    FranchiseId = franchiseA,
                    Date = date,
                    TotalRevenue = 5000000 + i * 100000,
                    TotalOrders = 40 + i
                });

                await repository.CreateDailyRevenueReportAsync(new DailyRevenueReport
                {
                    FranchiseId = franchiseB,
                    Date = date,
                    TotalRevenue = 3000000 + i * 50000,
                    TotalOrders = 25 + i
                });
            }

            // Seed Top Products
            var product1 = Guid.NewGuid();
            var product2 = Guid.NewGuid();

            for (int i = 0; i < 5; i++)
            {
                var date = today.AddDays(-i);

                await repository.CreateTopSellingProductReportAsync(new TopSellingProductReport
                {
                    FranchiseId = franchiseA,
                    Date = date,
                    ProductId = product1,
                    ProductName = "Capuccino",
                    QuantitySold = 100 + i * 5
                });

                await repository.CreateTopSellingProductReportAsync(new TopSellingProductReport
                {
                    FranchiseId = franchiseB,
                    Date = date,
                    ProductId = product2,
                    ProductName = "Latte",
                    QuantitySold = 80 + i * 2
                });
            }
        }
    }
}
