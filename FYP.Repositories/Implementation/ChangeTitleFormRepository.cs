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
    public class ChangeTitleFormRepository : IChangeTitleFormRepository
    {
        private readonly ApplicationDbContext _context;

        public ChangeTitleFormRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChangeTitleForm> GetByIdAsync(string id)
        {
            return await _context.ChangeTitleForms.FindAsync(id);
        }

        public async Task<IEnumerable<ChangeTitleForm>> GetAllAsync()
        {
            return await _context.ChangeTitleForms.ToListAsync();
        }

        public async Task AddAsync(ChangeTitleForm changeTitleForm)
        {
            _context.ChangeTitleForms.Add(changeTitleForm);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChangeTitleForm changeTitleForm)
        {
            _context.ChangeTitleForms.Update(changeTitleForm);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var changeTitleForm = await GetByIdAsync(id);
            if (changeTitleForm != null)
            {
                _context.ChangeTitleForms.Remove(changeTitleForm);
                await _context.SaveChangesAsync();
            }
        }
    }

}
