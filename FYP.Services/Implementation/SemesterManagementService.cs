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

namespace FYP.Services.Implementation
{
    public class SemesterManagementService : ISemesterManagementService
    {
        private readonly ISemesterManagementRepository _repository;

        public SemesterManagementService(ISemesterManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<SemesterManagement> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SemesterManagement>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(SemesterManagement SemesterManagement)
        {
            await _repository.AddAsync(SemesterManagement);
        }

        public async Task UpdateAsync(SemesterManagement SemesterManagement)
        {
            await _repository.UpdateAsync(SemesterManagement);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
