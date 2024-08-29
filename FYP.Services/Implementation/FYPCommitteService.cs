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
    public class FYPCommitteService : IFYPCommitteService
    {
        private readonly IFYPCommitteRepository _repository;

        public FYPCommitteService(IFYPCommitteRepository repository)
        {
            _repository = repository;
        }

        public async Task<FYPCommitte> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<FYPCommitte>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(FYPCommitte FYPCommitte)
        {
            await _repository.AddAsync(FYPCommitte);
        }

        public async Task UpdateAsync(FYPCommitte FYPCommitte)
        {
            await _repository.UpdateAsync(FYPCommitte);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
