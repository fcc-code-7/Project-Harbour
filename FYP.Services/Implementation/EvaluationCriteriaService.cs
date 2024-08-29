using FYP.Db;
using FYP.Entities;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services.Implementation
{
    public class EvaluationCriteriaService : IEvaluationCriteriaService
    {
        private readonly IEvaluationCriteriaRepository _repository;

        public EvaluationCriteriaService(IEvaluationCriteriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<EvaluationCriteria> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EvaluationCriteria>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(EvaluationCriteria EvaluationCriteria)
        {
            await _repository.AddAsync(EvaluationCriteria);
        }

        public async Task UpdateAsync(EvaluationCriteria EvaluationCriteria)
        {
            await _repository.UpdateAsync(EvaluationCriteria);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
