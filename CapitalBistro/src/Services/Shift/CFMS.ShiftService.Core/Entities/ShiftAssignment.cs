using System;
using CFMS.ShiftService.Core.Enums;

namespace CFMS.ShiftService.Core.Entities
{
    public class ShiftAssignment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ShiftConfigId { get; set; }
        public Guid UserId { get; set; }
        
        public DateOnly WorkingDate { get; set; }
        public AttendanceStatus Status { get; set; } = AttendanceStatus.Scheduled;

        public ShiftConfig ShiftConfig { get; set; }
    }
}
