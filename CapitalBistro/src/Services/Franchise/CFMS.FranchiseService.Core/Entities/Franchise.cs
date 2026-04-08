using System;
using CFMS.FranchiseService.Core.Enums;

namespace CFMS.FranchiseService.Core.Entities
{
    public class Franchise
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public FranchiseType Type { get; set; }
        public FranchiseStatus Status { get; set; } = FranchiseStatus.Prep;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
