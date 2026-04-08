using System;
using CFMS.PaymentService.Core.Enums;

namespace CFMS.PaymentService.API.DTOs
{
    public class PosPaymentRequest
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } // Cash or CreditCard
    }

    public class OnlinePaymentRequest
    {
        public Guid OrderId { get; set; }
        public Guid FranchiseId { get; set; } // Khách mua online thì FranchiseId đẩy từ đơn hàng xuống
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } // VNPay or MoMo
    }

    public class VNPayWebhookRequest
    {
        // Thông tin giả định của một gói Webhook
        public Guid TransactionId { get; set; }
        public string Vnp_TransactionNo { get; set; }
        public string Vnp_ResponseCode { get; set; } // '00' là thành công
        public string Signature { get; set; } // Chữ ký xác thực
    }
}
