using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CFMS.PaymentService.Core.Entities;
using CFMS.PaymentService.Core.Enums;
using CFMS.PaymentService.Core.Interfaces;
using CFMS.PaymentService.API.DTOs;

namespace CFMS.PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _repository;
        private readonly IPaymentGatewayService _gatewayService;

        public PaymentsController(IPaymentRepository repository, IPaymentGatewayService gatewayService)
        {
            _repository = repository;
            _gatewayService = gatewayService;
        }

        private Guid? GetFranchiseIdFromToken()
        {
            var fIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(fIdClaim)) return null;
            return Guid.Parse(fIdClaim);
        }

        private bool IsUserInRole(string role)
        {
            var roleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            return User.Claims.Any(c => c.Type == roleClaimType && c.Value.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        [HttpPost("pos")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<IActionResult> ProcessPosPayment([FromBody] PosPaymentRequest request)
        {
            try
            {
                var franchiseId = GetFranchiseIdFromToken();
                if (franchiseId == null) return Forbid();

                if (request.Method != PaymentMethod.Cash && request.Method != PaymentMethod.CreditCard)
                {
                    return BadRequest("Giao dịch POS chỉ hỗ trợ Cash hoặc CreditCard.");
                }

                var transaction = new PaymentTransaction
                {
                    OrderId = request.OrderId,
                    FranchiseId = franchiseId.Value,
                    Amount = request.Amount,
                    Method = request.Method,
                    Status = PaymentStatus.Success // Thu xong tại quầy là ghi nhận Success ngay
                };

                await _repository.CreateTransactionAsync(transaction);
                return Created($"/api/payments/{transaction.Id}", transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("online")]
        [Authorize] // Có thể là Khách hàng đã login
        public async Task<IActionResult> CreateOnlinePaymentUrl([FromBody] OnlinePaymentRequest request)
        {
            try
            {
                if (request.Method != PaymentMethod.VNPay && request.Method != PaymentMethod.MoMo)
                {
                    return BadRequest("API này chỉ hỗ trợ VNPay / MoMo.");
                }

                var transaction = new PaymentTransaction
                {
                    OrderId = request.OrderId,
                    FranchiseId = request.FranchiseId,
                    Amount = request.Amount,
                    Method = request.Method,
                    Status = PaymentStatus.Pending // Trạng thái Pending chờ Webhook
                };

                await _repository.CreateTransactionAsync(transaction);

                // Gọi Gateway tạo URL
                var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
                var paymentUrl = await _gatewayService.CreatePaymentUrlAsync(transaction, clientIp);

                return Ok(new
                {
                    TransactionId = transaction.Id,
                    PaymentUrl = paymentUrl
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("webhook/vnpay")]
        [AllowAnonymous] // Không cần thẻ Token, VNPay Server dội thằng vào đây
        public async Task<IActionResult> VNPayWebhookCallback([FromBody] VNPayWebhookRequest request)
        {
            try
            {
                // Bước Sinh tử 1: Validate xem có đúng đây là VNPay gởi lên không hay Hacker gởi FAKE
                var isValid = _gatewayService.ValidateSignature("fake_payload", request.Signature);
                if (!isValid)
                {
                    // Trả mã lỗi 97 (Chữ ký không hợp lệ) cho cổng thanh toán
                    return BadRequest(new { RspCode = "97", Message = "Invalid Signature" });
                }

                var transaction = await _repository.GetTransactionByIdAsync(request.TransactionId);
                if (transaction == null)
                    return BadRequest(new { RspCode = "01", Message = "Order not found" });

                if (transaction.Status != PaymentStatus.Pending)
                    return BadRequest(new { RspCode = "02", Message = "Order already confirmed" });

                // Vnp_ResponseCode == "00" có nghĩa là khách đã thanh toán OK xong trên App Ngân Hàng
                var newStatus = request.Vnp_ResponseCode == "00" ? PaymentStatus.Success : PaymentStatus.Failed;

                await _repository.UpdateTransactionStatusAsync(transaction.Id, newStatus, request.Vnp_TransactionNo);

                // Trả về mã thành công ('00') cho VNPay biết CFMS đã ghi nhận
                return Ok(new { RspCode = "00", Message = "Confirm Success" });
            }
            catch (Exception ex)
            {
                // Unknow Error
                return StatusCode(500, new { RspCode = "99", Message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (IsUserInRole("Manager") || IsUserInRole("Staff"))
                {
                    var franchiseId = GetFranchiseIdFromToken();
                    if (franchiseId == null) return Forbid();
                    
                    var data = await _repository.GetTransactionsAsync(franchiseId.Value, page, pageSize);
                    return Ok(data);
                }
                
                if (IsUserInRole("Admin"))
                {
                    var data = await _repository.GetTransactionsAsync(null, page, pageSize);
                    return Ok(data);
                }

                return Forbid("Bạn không có quyền xem báo cáo thanh toán.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
