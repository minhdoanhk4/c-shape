using System;
using System.Collections.Generic;

namespace CFMS.ShiftService.API.DTOs
{
    public class CreateShiftConfigRequest
    {
        public string ShiftName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int RequiredStaffCount { get; set; }
    }

    public class AssignShiftRequest
    {
        public Guid ShiftConfigId { get; set; }
        public DateOnly WorkingDate { get; set; }
        public List<Guid> UserIds { get; set; } = new List<Guid>();
    }
}
