using System;

namespace CFMS.CustomerService.API.DTOs
{
    public class CreateCustomerRequest
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class LoyaltyTransactionRequest
    {
        public Guid? OrderId { get; set; }
        public decimal PointsEarned { get; set; }
        public decimal PointsRedeemed { get; set; }
        public string Note { get; set; }
    }
}
