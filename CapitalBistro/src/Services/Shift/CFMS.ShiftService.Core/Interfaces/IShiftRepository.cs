using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.ShiftService.Core.Entities;

namespace CFMS.ShiftService.Core.Interfaces
{
    public interface IShiftRepository
    {
        Task<IEnumerable<ShiftConfig>> GetShiftConfigsAsync(Guid franchiseId);
        Task CreateShiftConfigAsync(ShiftConfig shiftConfig);
        Task<ShiftConfig> GetShiftConfigByIdAsync(Guid shiftConfigId);
        
        Task AssignShiftAsync(ShiftAssignment assignment);
        Task<IEnumerable<ShiftAssignment>> GetMyScheduleAsync(Guid userId, DateOnly fromDate, DateOnly toDate);
    }
}
