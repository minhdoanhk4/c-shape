using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.CustomerService.Core.Entities;

namespace CFMS.CustomerService.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<CustomerInfo> GetCustomerByIdAsync(Guid id);
        Task<CustomerInfo> GetCustomerByPhoneAsync(string phone);
        Task CreateCustomerAsync(CustomerInfo customer);
        Task AddLoyaltyTransactionAsync(LoyaltyTransaction transaction);
    }
}
