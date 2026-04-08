using System;
using System.Threading.Tasks;
using CFMS.PaymentService.Core.Entities;
using CFMS.PaymentService.Core.Interfaces;

namespace CFMS.PaymentService.Infrastructure.ExternalServices
{
    public class MockVNPayService : IPaymentGatewayService
    {
        // Giả lập logic sinh URL của VNPay. Trong thực tế sẽ cần nối chuỗi param, băm SHA256 với SecretKey...
        public Task<string> CreatePaymentUrlAsync(PaymentTransaction transaction, string clientIp)
        {
            var vnp_TmnCode = "MOCK_CODE";
            var vnp_ReturnUrl = $"https://capitalbistro.vn/api/payments/return"; // URL giả định web client
            
            // Giả lập Payload URL
            var url = $"https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?vnp_Amount={transaction.Amount * 100}&vnp_Command=pay&vnp_CreateDate={DateTime.UtcNow:yyyyMMddHHmmss}&vnp_CurrCode=VND&vnp_IpAddr={clientIp}&vnp_Locale=vn&vnp_OrderInfo=ThanhToanDonHang_{transaction.OrderId}&vnp_ReturnUrl={vnp_ReturnUrl}&vnp_TmnCode={vnp_TmnCode}&vnp_TxnRef={transaction.Id}&vnp_Version=2.1.0";

            return Task.FromResult(url);
        }

        // Trong thực tế, VNPay sẽ gửi kèm vnp_SecureHash. API của ta phải băm lại toàn bộ body và đem đi match với vnp_SecureHash
        public bool ValidateSignature(string payload, string signature)
        {
            // MOCK VALIDATION: Hệ thống bài bản sẽ tạo hàm băm HMACSHA512.
            // Để đơn giản test, giả sử cứ webhook nào gởi lên '123456789' làm signature thì tính là hàng Real.
            return signature == "123456789";
        }
    }
}
