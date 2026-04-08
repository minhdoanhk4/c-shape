using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using CFMS.OrderService.Core.Entities;
using CFMS.OrderService.Core.Enums;
using CFMS.OrderService.Core.Interfaces;
using CFMS.OrderService.API.DTOs;
using MassTransit;
using CFMS.Shared.Events;
using MassTransit.KafkaIntegration;

namespace CFMS.OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ITopicProducer<OrderCreatedEvent> _producer;

        public OrdersController(IOrderRepository repository, ITopicProducer<OrderCreatedEvent> producer)
        {
            _repository = repository;
            _producer = producer;
        }

        private Guid? GetFranchiseId()
        {
            var fIdClaim = User.Claims.FirstOrDefault(c => c.Type == "FranchiseId")?.Value;
            if (string.IsNullOrEmpty(fIdClaim)) return null;
            return Guid.Parse(fIdClaim);
        }

        private Guid? GetCustomerId()
        {
            var nameIdentifier = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(nameIdentifier)) return null;
            return Guid.Parse(nameIdentifier);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var franchiseId = GetFranchiseId();
            var orders = await _repository.GetOrdersAsync(franchiseId);
            return Ok(orders);
        }

        [HttpPost("pos")]
        public async Task<IActionResult> CreatePosOrder([FromBody] CreatePosOrderRequest request)
        {
            var franchiseId = GetFranchiseId();
            if (franchiseId == null)
            {
                return Forbid("Only Staff/Manager can create POS orders.");
            }

            var order = new Order
            {
                FranchiseId = franchiseId.Value,
                OrderType = OrderType.POS,
                Status = OrderStatus.Completed
            };

            foreach (var item in request.Items)
            {
                order.AddOrderItem(item.ProductId, item.Quantity, item.UnitPrice);
            }

            order.Validate();

            await _repository.CreateOrderAsync(order);
            if (await _repository.SaveChangesAsync())
            {
                // Produce Event
                await _producer.Produce(new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    FranchiseId = order.FranchiseId,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.OrderItems.Sum(x => x.SubTotal),
                    Items = order.OrderItems.Select(x => new OrderCreatedItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice
                    }).ToList()
                });

                return Created($"/api/orders/{order.Id}", order);
            }
            return BadRequest("Failed to create POS order.");
        }

        [HttpPost("online")]
        public async Task<IActionResult> CreateOnlineOrder([FromBody] CreateOnlineOrderRequest request)
        {
            var customerId = GetCustomerId();
            
            var order = new Order
            {
                FranchiseId = request.FranchiseId,
                CustomerId = customerId,
                OrderType = OrderType.Online,
                Status = OrderStatus.Pending
            };

            foreach (var item in request.Items)
            {
                order.AddOrderItem(item.ProductId, item.Quantity, item.UnitPrice);
            }

            order.Validate();

            await _repository.CreateOrderAsync(order);
            if (await _repository.SaveChangesAsync())
            {
                // Produce Event
                await _producer.Produce(new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    FranchiseId = order.FranchiseId,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.OrderItems.Sum(x => x.SubTotal),
                    Items = order.OrderItems.Select(x => new OrderCreatedItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice
                    }).ToList()
                });

                return Created($"/api/orders/{order.Id}", order);
            }
            return BadRequest("Failed to create Online order.");
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest request)
        {
            var order = await _repository.GetOrderByIdAsync(id);
            if (order == null) return NotFound("Order not found.");

            var franchiseId = GetFranchiseId();
            // HQ (franchiseId == null) hoặc Staff của chính chi nhánh đó mới được phép đổi status
            if (franchiseId != null && order.FranchiseId != franchiseId.Value)
            {
                return Forbid("You do not have permission to update this order.");
            }

            order.Status = request.Status;
            await _repository.UpdateOrderAsync(order);
            
            if (await _repository.SaveChangesAsync())
            {
                return Ok(order);
            }
            return BadRequest("Failed to update status.");
        }
    }
}
