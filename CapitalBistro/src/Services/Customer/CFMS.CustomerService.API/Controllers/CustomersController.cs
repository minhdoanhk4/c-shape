using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CFMS.CustomerService.Core.Entities;
using CFMS.CustomerService.Core.Interfaces;
using CFMS.CustomerService.API.DTOs;

namespace CFMS.CustomerService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repository;

        public CustomersController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        private Guid? GetFranchiseIdFromToken()
        {
            if (User.Identity is not { IsAuthenticated: true }) return null;
            var fIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(fIdClaim)) return null;
            return Guid.Parse(fIdClaim);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var existing = await _repository.GetCustomerByPhoneAsync(request.PhoneNumber);
            if (existing != null)
            {
                return BadRequest("Phone number already registered.");
            }

            var customer = new CustomerInfo
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth
            };

            await _repository.CreateCustomerAsync(customer);
            return Created($"/api/customers/{customer.Id}", customer);
        }

        [HttpGet("{phone}")]
        [Authorize]
        public async Task<IActionResult> GetCustomerByPhone(string phone)
        {
            var customer = await _repository.GetCustomerByPhoneAsync(phone);
            if (customer == null) return NotFound("Customer not found.");
            return Ok(customer);
        }

        [HttpPost("{id}/loyalty-transaction")]
        [Authorize]
        public async Task<IActionResult> AddLoyaltyTransaction(Guid id, [FromBody] LoyaltyTransactionRequest request)
        {
            var franchiseId = GetFranchiseIdFromToken();

            var transaction = new LoyaltyTransaction
            {
                CustomerId = id,
                FranchiseId = franchiseId, // Sẽ có value nếu Staff thuộc chi nhánh gọi, hoặc null nếu Admin gọi
                OrderId = request.OrderId,
                PointsEarned = request.PointsEarned,
                PointsRedeemed = request.PointsRedeemed,
                Note = request.Note
            };

            try
            {
                await _repository.AddLoyaltyTransactionAsync(transaction);
                
                // Trả về Customer hiện tại để POS thấy số điểm và hạng mới nhất
                var updatedCustomer = await _repository.GetCustomerByIdAsync(id);
                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
