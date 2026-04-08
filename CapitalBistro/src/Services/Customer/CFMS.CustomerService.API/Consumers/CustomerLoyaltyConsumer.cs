using System;
using System.Threading.Tasks;
using MassTransit;
using CFMS.Shared.Events;
using CFMS.CustomerService.Core.Interfaces;
using CFMS.CustomerService.Core.Entities;
using Microsoft.Extensions.Logging;

namespace CFMS.CustomerService.API.Consumers
{
    public class CustomerLoyaltyConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerLoyaltyConsumer> _logger;

        public CustomerLoyaltyConsumer(ICustomerRepository repository, ILogger<CustomerLoyaltyConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var @event = context.Message;

            if (@event.CustomerId == null)
            {
                _logger.LogInformation("Order {OrderId} has no CustomerId. Skipping loyalty points.", @event.OrderId);
                return;
            }

            _logger.LogInformation("Processing Loyalty for Customer: {CustomerId}, Order: {OrderId}", @event.CustomerId, @event.OrderId);

            try
            {
                var customer = await _repository.GetCustomerByIdAsync(@event.CustomerId.Value);
                if (customer == null)
                {
                    _logger.LogWarning("Customer {CustomerId} not found.", @event.CustomerId);
                    return;
                }

                // Tính điểm: 1 điểm cho mỗi 10,000 VNĐ (GIả định)
                int pointsEarned = (int)(@event.TotalAmount / 10000);
                
                if (pointsEarned > 0)
                {
                    customer.AvailablePoints += pointsEarned;
                    customer.TotalAccumulatedPoints += pointsEarned;
                    
                    await _repository.AddLoyaltyTransactionAsync(new LoyaltyTransaction
                    {
                        CustomerId = customer.Id,
                        PointsEarned = pointsEarned,
                        Note = $"Points earned from Order {@event.OrderId}",
                        TransactionDate = DateTime.UtcNow
                    });

                    _logger.LogInformation("Added {Points} points to Customer {CustomerId}", pointsEarned, customer.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Loyalty for Order {OrderId}", @event.OrderId);
                throw;
            }
        }
    }
}
