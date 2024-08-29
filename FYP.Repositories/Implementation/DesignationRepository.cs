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
    public class DesignationRepository : IDesignationRepository
    {
        private readonly ApplicationDbContext _context;

        public DesignationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Designation> GetByIdAsync(string id)
        {
            return await _context.Designations.FindAsync(id);
        }

        public async Task<IEnumerable<Designation>> GetAllAsync()
        {
            return await _context.Designations.ToListAsync();
        }

        public async Task AddAsync(Designation Designation)
        {
            _context.Designations.Add(Designation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Designation Designation)
        {
            _context.Designations.Update(Designation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var Designation = await GetByIdAsync(id);
            if (Designation != null)
            {
                _context.Designations.Remove(Designation);
                await _context.SaveChangesAsync();
            }
        }
    }

}
