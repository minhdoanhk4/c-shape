using System;
using System.Threading.Tasks;
using CFMS.PaymentService.Core.Entities;

namespace CFMS.PaymentService.Core.Interfaces
{
    public interface IPaymentGatewayService
    {
        Task<string> CreatePaymentUrlAsync(PaymentTransaction transaction, string clientIp);
        bool ValidateSignature(string payload, string signature);
    }
}
