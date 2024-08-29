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
    public class SupervisorService : ISupervisorService
    {
        private readonly ISupervisorRepository _repository;

        public SupervisorService(ISupervisorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Supervisor> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Supervisor>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(Supervisor Supervisor)
        {
            await _repository.AddAsync(Supervisor);
        }

        public async Task UpdateAsync(Supervisor Supervisor)
        {
            await _repository.UpdateAsync(Supervisor);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
