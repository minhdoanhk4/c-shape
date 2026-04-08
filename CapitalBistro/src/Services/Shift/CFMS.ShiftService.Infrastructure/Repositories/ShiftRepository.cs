using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.ShiftService.Core.Entities;
using CFMS.ShiftService.Core.Exceptions;
using CFMS.ShiftService.Core.Interfaces;
using CFMS.ShiftService.Infrastructure.Data;

namespace CFMS.ShiftService.Infrastructure.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly ShiftDbContext _context;

        public ShiftRepository(ShiftDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShiftConfig>> GetShiftConfigsAsync(Guid franchiseId)
        {
            return await _context.ShiftConfigs
                .Where(s => s.FranchiseId == franchiseId && s.IsActive)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task CreateShiftConfigAsync(ShiftConfig shiftConfig)
        {
            await _context.ShiftConfigs.AddAsync(shiftConfig);
            await _context.SaveChangesAsync();
        }

        public async Task<ShiftConfig> GetShiftConfigByIdAsync(Guid shiftConfigId)
        {
            return await _context.ShiftConfigs.FindAsync(shiftConfigId);
        }

        public async Task AssignShiftAsync(ShiftAssignment assignment)
        {
            var targetShift = await _context.ShiftConfigs.FindAsync(assignment.ShiftConfigId);
            if (targetShift == null) throw new Exception("Shift Config not found.");

            // LOGIC KIỂM TRA TRÙNG LỊCH (OVERLAP)
            // Lấy tất cả ca làm việc trong cùng 1 ngày của User này
            var existingAssignments = await _context.ShiftAssignments
                .Include(a => a.ShiftConfig)
                .Where(a => a.UserId == assignment.UserId && a.WorkingDate == assignment.WorkingDate)
                .ToListAsync();

            foreach (var existing in existingAssignments)
            {
                var existingShift = existing.ShiftConfig;
                
                // Thuật toán phát hiện đoạn thẳng giao thoa (Overlap Time)
                // Hai khoảng (Start1, End1) và (Start2, End2) giao nhau khi: Max(Start1, Start2) < Min(End1, End2)
                var maxStart = targetShift.StartTime > existingShift.StartTime ? targetShift.StartTime : existingShift.StartTime;
                var minEnd = targetShift.EndTime < existingShift.EndTime ? targetShift.EndTime : existingShift.EndTime;

                if (maxStart < minEnd) // Xảy ra giao thoa
                {
                    throw new OverlapShiftException($"Xung đột ca làm việc! Nhân viên này đã được phân vào ca '{existingShift.ShiftName}' ({existingShift.StartTime} - {existingShift.EndTime}) trong cùng ngày.");
                }
            }

            // Nếu an toàn, tiến hành gán ca
            await _context.ShiftAssignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShiftAssignment>> GetMyScheduleAsync(Guid userId, DateOnly fromDate, DateOnly toDate)
        {
            return await _context.ShiftAssignments
                .Include(a => a.ShiftConfig)
                .Where(a => a.UserId == userId && a.WorkingDate >= fromDate && a.WorkingDate <= toDate)
                .OrderBy(a => a.WorkingDate)
                .ThenBy(a => a.ShiftConfig.StartTime)
                .ToListAsync();
        }
    }
}
