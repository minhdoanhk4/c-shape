using System;
using CFMS.DeliveryService.Core.Enums;

namespace CFMS.DeliveryService.API.DTOs
{
    public class CreateDeliveryRequest
    {
        public Guid ReferenceId { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public Guid? FranchiseId { get; set; } // Tuỳ chọn, nếu Manager tạo sẽ lấy từ JWT. Nếu Admin tạo phải nhập tay.
        
        public string PickupAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }
    }

    public class AssignShipperRequest
    {
        public Guid ShipperId { get; set; }
    }

    public class UpdateDeliveryStatusRequest
    {
        public DeliveryStatus Status { get; set; }
    }
}
