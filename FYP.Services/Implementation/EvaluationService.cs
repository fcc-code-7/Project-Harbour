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
    public class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationRepository _repository;

        public EvaluationService(IEvaluationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Evaluation> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Evaluation>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(Evaluation Evaluation)
        {
            await _repository.AddAsync(Evaluation);
        }

        public async Task UpdateAsync(Evaluation Evaluation)
        {
            await _repository.UpdateAsync(Evaluation);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
