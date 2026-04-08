using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CFMS.FranchiseService.Core.Entities;
using CFMS.FranchiseService.Core.Interfaces;
using CFMS.FranchiseService.Infrastructure.Data;

namespace CFMS.FranchiseService.Infrastructure.Repositories
{
    public class FranchiseRepository : IFranchiseRepository
    {
        private readonly FranchiseDbContext _context;

        public FranchiseRepository(FranchiseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Franchise>> GetAllFranchisesAsync()
        {
            return await _context.Franchises.ToListAsync();
        }

        public async Task<Franchise> GetFranchiseByIdAsync(Guid id)
        {
            return await _context.Franchises.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task CreateFranchiseAsync(Franchise franchise)
        {
            await _context.Franchises.AddAsync(franchise);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFranchiseAsync(Franchise franchise)
        {
            _context.Update(franchise);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
