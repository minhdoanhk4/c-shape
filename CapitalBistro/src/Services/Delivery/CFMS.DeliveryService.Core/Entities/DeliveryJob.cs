using System;
using CFMS.DeliveryService.Core.Enums;

namespace CFMS.DeliveryService.Core.Entities
{
    public class DeliveryJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        // ID tham chiếu tới Order (dành cho OnlineOrder) hoặc TransferReport (dành cho InternalTransfer)
        public Guid ReferenceId { get; set; } 
        
        public DeliveryType DeliveryType { get; set; }
        
        // Chi nhánh liên quan (Gửi đi hoặc Nhận về)
        public Guid FranchiseId { get; set; }
        
        // Người giao hàng (Nhân viên nội bộ có Role Shipper hoặc đối tác)
        public Guid? ShipperId { get; set; }
        
        public DeliveryStatus Status { get; set; } = DeliveryStatus.Pending;

        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
