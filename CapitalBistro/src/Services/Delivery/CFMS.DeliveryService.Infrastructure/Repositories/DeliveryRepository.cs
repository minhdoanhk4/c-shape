using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.DeliveryService.Core.Entities;
using CFMS.DeliveryService.Core.Enums;
using CFMS.DeliveryService.Core.Interfaces;
using CFMS.DeliveryService.Infrastructure.Data;

namespace CFMS.DeliveryService.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DeliveryDbContext _context;

        public DeliveryRepository(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeliveryJob>> GetAllDeliveriesAsync()
        {
            return await _context.DeliveryJobs.OrderByDescending(d => d.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<DeliveryJob>> GetDeliveriesByFranchiseAsync(Guid franchiseId)
        {
            return await _context.DeliveryJobs
                .Where(d => d.FranchiseId == franchiseId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DeliveryJob>> GetDeliveriesByShipperAsync(Guid shipperId)
        {
            return await _context.DeliveryJobs
                .Where(d => d.ShipperId == shipperId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<DeliveryJob> GetDeliveryByIdAsync(Guid id)
        {
            return await _context.DeliveryJobs.FindAsync(id);
        }

        public async Task CreateDeliveryAsync(DeliveryJob job)
        {
            job.CreatedAt = DateTime.UtcNow;
            await _context.DeliveryJobs.AddAsync(job);
            await _context.SaveChangesAsync();
        }

        public async Task AssignShipperAsync(Guid deliveryId, Guid shipperId)
        {
            var job = await _context.DeliveryJobs.FindAsync(deliveryId);
            if (job == null) throw new Exception("Delivery Job not found.");

            job.ShipperId = shipperId;
            job.Status = DeliveryStatus.Assigned;
            
            _context.DeliveryJobs.Update(job);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid deliveryId, DeliveryStatus newStatus)
        {
            var job = await _context.DeliveryJobs.FindAsync(deliveryId);
            if (job == null) throw new Exception("Delivery Job not found.");

            job.Status = newStatus;
            
            // Tự động chốt sổ thời gian giao xong
            if (newStatus == DeliveryStatus.Delivered)
            {
                job.ActualDeliveryTime = DateTime.UtcNow;
            }

            _context.DeliveryJobs.Update(job);
            await _context.SaveChangesAsync();
        }
    }
}
