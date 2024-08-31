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

namespace FYP.Services
{
    public class ChangeSupervisorFormService : IChangeSupervisorFormService
    {
        private readonly IChangeSupervisorFormRepository _repository;

        public ChangeSupervisorFormService(IChangeSupervisorFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<ChangeSupervisorForm> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ChangeSupervisorForm>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(ChangeSupervisorForm ChangeSupervisorForm)
        {
            await _repository.AddAsync(ChangeSupervisorForm);
        }

        public async Task UpdateAsync(ChangeSupervisorForm ChangeSupervisorForm)
        {
            await _repository.UpdateAsync(ChangeSupervisorForm);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
