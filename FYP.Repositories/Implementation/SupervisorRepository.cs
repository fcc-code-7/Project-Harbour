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
    public class SupervisorRepository : ISupervisorRepository
    {
        private readonly ApplicationDbContext _context;

        public SupervisorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Supervisor> GetByIdAsync(string id)
        {
            return await _context.Supervisors.FindAsync(id);
        }

        public async Task<IEnumerable<Supervisor>> GetAllAsync()
        {
            return await _context.Supervisors.ToListAsync();
        }

        public async Task AddAsync(Supervisor Supervisor)
        {
            _context.Supervisors.Add(Supervisor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Supervisor Supervisor)
        {
            _context.Supervisors.Update(Supervisor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var Supervisor = await GetByIdAsync(id);
            if (Supervisor != null)
            {
                _context.Supervisors.Remove(Supervisor);
                await _context.SaveChangesAsync();
            }
        }

      
    }

}
