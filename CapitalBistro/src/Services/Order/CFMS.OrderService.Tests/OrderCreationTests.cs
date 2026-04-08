using System;
using CFMS.OrderService.Core.Entities;
using CFMS.OrderService.Core.Enums;
using FluentAssertions;
using Xunit;

namespace CFMS.OrderService.Tests
{
    public class OrderCreationTests
    {
        [Fact]
        public void CreateOrder_WithMultipleItems_ShouldCalculateCorrectTotalAmount()
        {
            // Arrange
            var order = new Order
            {
                FranchiseId = Guid.NewGuid(),
                OrderType = OrderType.POS,
                Status = OrderStatus.Completed
            };

            // Act
            // Món A: 50,000đ x 1
            order.AddOrderItem(Guid.NewGuid(), 1, 50000m);
            // Món B: 30,000đ x 2
            order.AddOrderItem(Guid.NewGuid(), 2, 30000m);

            // Assert
            // 50,000 + (30,000 * 2) = 110,000
            order.TotalAmount.Should().Be(110000m);
            order.OrderItems.Should().HaveCount(2);
        }

        [Fact]
        public void ValidateOrder_WithNoItems_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var order = new Order
            {
                FranchiseId = Guid.NewGuid(),
                OrderType = OrderType.POS,
                Status = OrderStatus.Completed
            };

            // Act
            Action act = () => order.Validate();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Order must have at least one item.");
        }
    }
}
