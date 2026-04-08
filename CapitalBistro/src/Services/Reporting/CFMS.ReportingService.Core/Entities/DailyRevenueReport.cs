using System;

namespace CFMS.ReportingService.Core.Entities
{
    public class DailyRevenueReport
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FranchiseId { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }
}
