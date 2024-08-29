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
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _repository;

        public DesignationService(IDesignationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Designation> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Designation>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(Designation Designation)
        {
            await _repository.AddAsync(Designation);
        }

        public async Task UpdateAsync(Designation Designation)
        {
            await _repository.UpdateAsync(Designation);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
