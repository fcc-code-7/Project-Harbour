using FYP.Db;
using FYP.Entities;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Implementation
{
    public class FYPCommitteRepository : IFYPCommitteRepository
    {
        private readonly ApplicationDbContext _context;

        public FYPCommitteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FYPCommitte> GetByIdAsync(string id)
        {
            return await _context.FYPCommittes.FindAsync(id);
        }

        public async Task<IEnumerable<FYPCommitte>> GetAllAsync()
        {
            return await _context.FYPCommittes.ToListAsync();
        }

        public async Task AddAsync(FYPCommitte FYPCommitte)
        {
            _context.FYPCommittes.Add(FYPCommitte);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FYPCommitte FYPCommitte)
        {
            _context.FYPCommittes.Update(FYPCommitte);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var FYPCommitte = await GetByIdAsync(id);
            if (FYPCommitte != null)
            {
                _context.FYPCommittes.Remove(FYPCommitte);
                await _context.SaveChangesAsync();
            }
        }
    }

}
