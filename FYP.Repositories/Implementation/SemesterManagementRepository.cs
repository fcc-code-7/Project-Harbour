using FYP.Db;
using FYP.Entities;
using FYP.Models;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Implementation
{
    public class SemesterManagementRepository : ISemesterManagementRepository
    {
        private readonly ApplicationDbContext _context;

        public SemesterManagementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SemesterManagement> GetByIdAsync(string id)
        {
            return await _context.SemesterManagements.FindAsync(id);
        }

        public async Task<IEnumerable<SemesterManagement>> GetAllAsync()
        {
            return await _context.SemesterManagements.ToListAsync();
        }

        public async Task AddAsync(SemesterManagement SemesterManagement)
        {
            _context.SemesterManagements.Add(SemesterManagement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SemesterManagement SemesterManagement)
        {
            _context.SemesterManagements.Update(SemesterManagement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var SemesterManagement = await GetByIdAsync(id);
            if (SemesterManagement != null)
            {
                _context.SemesterManagements.Remove(SemesterManagement);
                await _context.SaveChangesAsync();
            }
        }
    }

}
