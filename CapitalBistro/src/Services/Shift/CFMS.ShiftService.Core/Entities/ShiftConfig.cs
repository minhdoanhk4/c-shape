using System;
using System.Collections.Generic;

namespace CFMS.ShiftService.Core.Entities
{
    public class ShiftConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FranchiseId { get; set; }
        
        public string ShiftName { get; set; } // Morning, Afternoon, Night
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        public int RequiredStaffCount { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<ShiftAssignment> Assignments { get; set; } = new List<ShiftAssignment>();
    }
}
