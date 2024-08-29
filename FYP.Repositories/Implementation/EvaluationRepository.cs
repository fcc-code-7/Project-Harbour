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
    public class EvaluationRepository : IEvaluationRepository
    {
        private readonly ApplicationDbContext _context;

        public EvaluationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Evaluation> GetByIdAsync(string id)
        {
            return await _context.Evaluations.FindAsync(id);
        }

        public async Task<IEnumerable<Evaluation>> GetAllAsync()
        {
            return await _context.Evaluations.ToListAsync();
        }

        public async Task AddAsync(Evaluation Evaluation)
        {
            _context.Evaluations.Add(Evaluation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Evaluation Evaluation)
        {
            _context.Evaluations.Update(Evaluation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var Evaluation = await GetByIdAsync(id);
            if (Evaluation != null)
            {
                _context.Evaluations.Remove(Evaluation);
                await _context.SaveChangesAsync();
            }
        }
    }

}
