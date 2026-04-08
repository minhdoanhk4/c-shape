using System;

namespace CFMS.ReportingService.API.DTOs
{
    public class RevenueReportResponse
    {
        public Guid FranchiseId { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }

    public class TopProductResponse
    {
        public Guid FranchiseId { get; set; }
        public DateOnly Date { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }
}
