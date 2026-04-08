using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CFMS.ShiftService.Core.Entities;
using CFMS.ShiftService.Core.Exceptions;
using CFMS.ShiftService.Core.Interfaces;
using CFMS.ShiftService.API.DTOs;

namespace CFMS.ShiftService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftRepository _repository;

        public ShiftsController(IShiftRepository repository)
        {
            _repository = repository;
        }

        private Guid GetFranchiseIdFromToken()
        {
            var fIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(fIdClaim))
                throw new UnauthorizedAccessException("Bạn không thuộc chi nhánh nào (FranchiseId missing).");
            return Guid.Parse(fIdClaim);
        }

        private Guid GetUserIdFromToken()
        {
            var nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(nameIdentifier))
                throw new UnauthorizedAccessException("Không tìm thấy ID người dùng trong Token.");
            return Guid.Parse(nameIdentifier);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateShiftConfig([FromBody] CreateShiftConfigRequest request)
        {
            try
            {
                var franchiseId = GetFranchiseIdFromToken();
                var config = new ShiftConfig
                {
                    FranchiseId = franchiseId,
                    ShiftName = request.ShiftName,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    RequiredStaffCount = request.RequiredStaffCount
                };

                await _repository.CreateShiftConfigAsync(config);
                return Created($"/api/shifts/{config.Id}", config);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetShiftConfigs()
        {
            try
            {
                var franchiseId = GetFranchiseIdFromToken();
                var configs = await _repository.GetShiftConfigsAsync(franchiseId);
                return Ok(configs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AssignShift([FromBody] AssignShiftRequest request)
        {
            try
            {
                // Verify quền của Manager trên cài đặt ca làm việc này
                var shiftConfig = await _repository.GetShiftConfigByIdAsync(request.ShiftConfigId);
                if (shiftConfig == null) return NotFound("Không tìm thấy ca làm việc.");
                
                var franchiseId = GetFranchiseIdFromToken();
                if (shiftConfig.FranchiseId != franchiseId)
                    return Forbid("Bạn không có quyền xếp ca làm việc của cửa hàng khác!");

                var successCount = 0;
                var errors = new System.Collections.Generic.List<string>();

                foreach (var userId in request.UserIds)
                {
                    var assignment = new ShiftAssignment
                    {
                        ShiftConfigId = request.ShiftConfigId,
                        UserId = userId,
                        WorkingDate = request.WorkingDate
                    };

                    try
                    {
                        await _repository.AssignShiftAsync(assignment);
                        successCount++;
                    }
                    catch (OverlapShiftException overlapEx)
                    {
                        errors.Add($"User {userId}: {overlapEx.Message}");
                    }
                }

                return Ok(new {
                    Message = $"Thành công phân ca cho {successCount}/{request.UserIds.Count} nhân viên.",
                    Errors = errors
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("my-schedule")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetMySchedule([FromQuery] string fromDate, [FromQuery] string toDate)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var from = DateOnly.Parse(fromDate);
                var to = DateOnly.Parse(toDate);

                var schedule = await _repository.GetMyScheduleAsync(userId, from, to);
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
