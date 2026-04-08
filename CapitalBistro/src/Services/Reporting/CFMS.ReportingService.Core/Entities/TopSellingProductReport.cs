using System;

namespace CFMS.ReportingService.Core.Entities
{
    public class TopSellingProductReport
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FranchiseId { get; set; }
        public DateOnly Date { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }
}
