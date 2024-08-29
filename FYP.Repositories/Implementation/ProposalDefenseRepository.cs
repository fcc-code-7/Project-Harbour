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
    public class ProposalDefenseRepository : IProposalDefenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ProposalDefenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProposalDefense> GetByIdAsync(string id)
        {
            return await _context.ProposalDefenses.FindAsync(id);
        }

        public async Task<IEnumerable<ProposalDefense>> GetAllAsync()
        {
            return await _context.ProposalDefenses.ToListAsync();
        }

        public async Task AddAsync(ProposalDefense ProposalDefense)
        {
            _context.ProposalDefenses.Add(ProposalDefense);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProposalDefense ProposalDefense)
        {
            _context.ProposalDefenses.Update(ProposalDefense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var ProposalDefense = await GetByIdAsync(id);
            if (ProposalDefense != null)
            {
                _context.ProposalDefenses.Remove(ProposalDefense);
                await _context.SaveChangesAsync();
            }
        }
    }

}
