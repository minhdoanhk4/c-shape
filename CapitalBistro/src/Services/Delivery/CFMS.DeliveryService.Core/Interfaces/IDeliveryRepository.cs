using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.DeliveryService.Core.Entities;
using CFMS.DeliveryService.Core.Enums;

namespace CFMS.DeliveryService.Core.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<IEnumerable<DeliveryJob>> GetAllDeliveriesAsync();
        Task<IEnumerable<DeliveryJob>> GetDeliveriesByFranchiseAsync(Guid franchiseId);
        Task<IEnumerable<DeliveryJob>> GetDeliveriesByShipperAsync(Guid shipperId);
        
        Task<DeliveryJob> GetDeliveryByIdAsync(Guid id);
        
        Task CreateDeliveryAsync(DeliveryJob job);
        Task AssignShipperAsync(Guid deliveryId, Guid shipperId);
        Task UpdateStatusAsync(Guid deliveryId, DeliveryStatus newStatus);
    }
}
