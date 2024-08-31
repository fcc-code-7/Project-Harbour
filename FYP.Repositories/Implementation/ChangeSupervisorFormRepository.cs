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
    public class ChangeSupervisorFormRepository : IChangeSupervisorFormRepository
    {
        private readonly ApplicationDbContext _context;

        public ChangeSupervisorFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChangeSupervisorForm> GetByIdAsync(string id)
        {
            return await _context.changeSupervisorForms.FindAsync(id);
        }

        public async Task<IEnumerable<ChangeSupervisorForm>> GetAllAsync()
        {
            return await _context.changeSupervisorForms.ToListAsync();
        }

        public async Task AddAsync(ChangeSupervisorForm ChangeSupervisorForm)
        {
            _context.changeSupervisorForms.Add(ChangeSupervisorForm);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChangeSupervisorForm ChangeSupervisorForm)
        {
            _context.changeSupervisorForms.Update(ChangeSupervisorForm);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var ChangeSupervisorForm = await GetByIdAsync(id);
            if (ChangeSupervisorForm != null)
            {
                _context.changeSupervisorForms.Remove(ChangeSupervisorForm);
                await _context.SaveChangesAsync();
            }
        }
    }

}
