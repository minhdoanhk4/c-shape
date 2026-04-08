using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CFMS.FranchiseService.Core.Entities;
using CFMS.FranchiseService.Core.Interfaces;
using CFMS.FranchiseService.API.DTOs;
using CFMS.FranchiseService.Core.Enums;

namespace CFMS.FranchiseService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FranchisesController : ControllerBase
    {
        private readonly IFranchiseRepository _repository;

        public FranchisesController(IFranchiseRepository repository)
        {
            _repository = repository;
        }

        private Guid? GetUserFranchiseId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(claim)) return null;
            return Guid.Parse(claim);
        }

        private bool IsAdmin()
        {
            var roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            return User.Claims.Any(c => c.Type == roleClaimType && c.Value.Equals("Admin", StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet]
        public async Task<IActionResult> GetFranchises()
        {
            var franchises = await _repository.GetAllFranchisesAsync();
            
            // Nếu không phải Admin, chỉ thấy các chi nhánh đang Active (đối với Client App/Khách hàng)
            if (!IsAdmin())
            {
                franchises = franchises.Where(f => f.Status == FranchiseStatus.Active);
            }

            var response = franchises.Select(MapToResponse);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFranchise(Guid id)
        {
            // Phân quyền: Manager chỉ được xem chi nhánh của mình
            if (!IsAdmin())
            {
                var userFranchiseId = GetUserFranchiseId();
                if (userFranchiseId != id) return Forbid("You only have access to your own franchise details.");
            }

            var franchise = await _repository.GetFranchiseByIdAsync(id);
            if (franchise == null) return NotFound();

            return Ok(MapToResponse(franchise));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFranchise([FromBody] CreateFranchiseRequest request)
        {
            var franchise = new Franchise
            {
                Name = request.Name,
                Address = request.Address,
                ContactPhone = request.ContactPhone,
                OpenTime = TimeSpan.Parse(request.OpenTime),
                CloseTime = TimeSpan.Parse(request.CloseTime),
                Type = request.Type,
                Status = FranchiseStatus.Prep
            };

            await _repository.CreateFranchiseAsync(franchise);
            return CreatedAtAction(nameof(GetFranchise), new { id = franchise.Id }, MapToResponse(franchise));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFranchise(Guid id, [FromBody] UpdateFranchiseRequest request)
        {
            var franchise = await _repository.GetFranchiseByIdAsync(id);
            if (franchise == null) return NotFound();

            franchise.Name = request.Name;
            franchise.Address = request.Address;
            franchise.ContactPhone = request.ContactPhone;
            franchise.OpenTime = TimeSpan.Parse(request.OpenTime);
            franchise.CloseTime = TimeSpan.Parse(request.CloseTime);
            franchise.Status = request.Status;

            await _repository.UpdateFranchiseAsync(franchise);
            return NoContent();
        }

        private FranchiseResponse MapToResponse(Franchise f)
        {
            return new FranchiseResponse
            {
                Id = f.Id,
                Name = f.Name,
                Address = f.Address,
                ContactPhone = f.ContactPhone,
                OpenTime = f.OpenTime.ToString(@"hh\:mm"),
                CloseTime = f.CloseTime.ToString(@"hh\:mm"),
                Type = f.Type,
                Status = f.Status,
                CreatedAt = f.CreatedAt
            };
        }
    }
}
