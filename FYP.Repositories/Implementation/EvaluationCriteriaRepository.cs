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
    public class EvaluationCriteriaRepository : IEvaluationCriteriaRepository
    {
        private readonly ApplicationDbContext _context;

        public EvaluationCriteriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EvaluationCriteria> GetByIdAsync(string id)
        {
            return await _context.EvaluationCriterias.FindAsync(id);
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetAllAsync()
        {
            return await _context.EvaluationCriterias.ToListAsync();
        }

        public async Task AddAsync(EvaluationCriteria evaluationCriteria)
        {
            _context.EvaluationCriterias.Add(evaluationCriteria);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EvaluationCriteria evaluationCriteria)
        {
            _context.EvaluationCriterias.Update(evaluationCriteria);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var evaluationCriteria = await GetByIdAsync(id);
            if (evaluationCriteria != null)
            {
                _context.EvaluationCriterias.Remove(evaluationCriteria);
                await _context.SaveChangesAsync();
            }
        }
    }

}
