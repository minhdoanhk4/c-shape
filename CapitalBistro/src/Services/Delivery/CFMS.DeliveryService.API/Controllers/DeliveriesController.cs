using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CFMS.DeliveryService.Core.Entities;
using CFMS.DeliveryService.Core.Enums;
using CFMS.DeliveryService.Core.Interfaces;
using CFMS.DeliveryService.API.DTOs;

namespace CFMS.DeliveryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryRepository _repository;

        public DeliveriesController(IDeliveryRepository repository)
        {
            _repository = repository;
        }

        private Guid? GetFranchiseIdFromToken()
        {
            var fIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(fIdClaim)) return null;
            return Guid.Parse(fIdClaim);
        }

        private bool IsUserInRole(string role)
        {
            // So sánh chuẩn Role khai báo trong Claim Scheme của .NET
            var roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            return User.Claims.Any(c => c.Type == roleClaimType && c.Value.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        private Guid GetUserIdFromToken()
        {
            var nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(nameIdentifier))
                throw new UnauthorizedAccessException("Không tìm thấy ID người dùng.");
            return Guid.Parse(nameIdentifier);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDelivery([FromBody] CreateDeliveryRequest request)
        {
            try
            {
                var franchiseIdToken = GetFranchiseIdFromToken();
                // Ưu tiên dùng token, nếu Admin tạo thì lấy từ payload
                var franchiseId = franchiseIdToken ?? request.FranchiseId; 
                
                if (franchiseId == null)
                    return BadRequest("Yêu cầu nhập FranchiseId.");

                var job = new DeliveryJob
                {
                    ReferenceId = request.ReferenceId,
                    DeliveryType = request.DeliveryType,
                    FranchiseId = franchiseId.Value,
                    PickupAddress = request.PickupAddress,
                    DeliveryAddress = request.DeliveryAddress,
                    EstimatedDeliveryTime = request.EstimatedDeliveryTime
                };

                await _repository.CreateDeliveryAsync(job);
                return Created($"/api/deliveries/{job.Id}", job);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliveries()
        {
            try
            {
                if (IsUserInRole("Shipper"))
                {
                    var shipperId = GetUserIdFromToken();
                    var jobs = await _repository.GetDeliveriesByShipperAsync(shipperId);
                    return Ok(jobs);
                }
                
                if (IsUserInRole("Manager") || IsUserInRole("Staff"))
                {
                    var franchiseId = GetFranchiseIdFromToken();
                    if (franchiseId == null) return Forbid();
                    var jobs = await _repository.GetDeliveriesByFranchiseAsync(franchiseId.Value);
                    return Ok(jobs);
                }

                // Nếu là Admin
                if (IsUserInRole("Admin"))
                {
                    var jobs = await _repository.GetAllDeliveriesAsync();
                    return Ok(jobs);
                }

                return Forbid("Tài khoản của bạn chưa được cấp quyền truy cập tính năng Giao hàng.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/assign")]
        [Authorize(Roles = "Manager,Admin")] // Chi quan ly/admin moi duoc xep Shipper
        public async Task<IActionResult> AssignShipper(Guid id, [FromBody] AssignShipperRequest request)
        {
            try
            {
                await _repository.AssignShipperAsync(id, request.ShipperId);
                return Ok($"Phân công chuyến hàng {id} cho Shipper thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/status")]
        // [Authorize(Roles = "Shipper,Manager,Admin")] // Nhieu ben check nen logic se nam trong Action
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateDeliveryStatusRequest request)
        {
            try
            {
                var job = await _repository.GetDeliveryByIdAsync(id);
                if (job == null) return NotFound("Delivery Job không tồn tại.");

                // Shipper Update -> Chỉ được phép cập nhật nếu ShipperId match
                if (IsUserInRole("Shipper"))
                {
                    var shipperId = GetUserIdFromToken();
                    if (job.ShipperId != shipperId)
                        return Forbid("Bạn không thể cập nhật đơn hàng của Shipper khác.");
                }
                // Manager/Staff Update -> Phải đúng Franchise
                else if (IsUserInRole("Manager") || IsUserInRole("Staff"))
                {
                    var franchiseId = GetFranchiseIdFromToken();
                    if (job.FranchiseId != franchiseId)
                        return Forbid("Chi nhánh của bạn không có quyền sở hữu đơn hàng này.");
                }

                await _repository.UpdateStatusAsync(id, request.Status);
                return Ok(new { Message = "Cập nhật trạng thái giao hàng thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
