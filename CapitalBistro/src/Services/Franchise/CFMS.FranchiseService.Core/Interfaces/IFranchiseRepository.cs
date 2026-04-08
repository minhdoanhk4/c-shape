using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CFMS.FranchiseService.Core.Entities;

namespace CFMS.FranchiseService.Core.Interfaces
{
    public interface IFranchiseRepository
    {
        Task<IEnumerable<Franchise>> GetAllFranchisesAsync();
        Task<Franchise> GetFranchiseByIdAsync(Guid id);
        Task CreateFranchiseAsync(Franchise franchise);
        Task UpdateFranchiseAsync(Franchise franchise);
        Task<bool> SaveChangesAsync();
    }
}
