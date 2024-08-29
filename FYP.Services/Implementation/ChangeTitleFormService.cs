using FYP.Db;
using FYP.Entities;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public class ChangeTitleFormService : IChangeTitleFormService
    {
        private readonly IChangeTitleFormRepository _repository;

        public ChangeTitleFormService(IChangeTitleFormRepository repository)
        {
            _repository = repository;
        }

        public async Task<ChangeTitleForm> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ChangeTitleForm>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(ChangeTitleForm ChangeTitleForm)
        {
            await _repository.AddAsync(ChangeTitleForm);
        }

        public async Task UpdateAsync(ChangeTitleForm ChangeTitleForm)
        {
            await _repository.UpdateAsync(ChangeTitleForm);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
