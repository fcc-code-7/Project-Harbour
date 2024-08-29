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
    public class ProjectCategoryService : IProjectCategoryService
    {
        private readonly IProjectCategoryRepository _repository;

        public ProjectCategoryService(IProjectCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectCategory> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ProjectCategory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(ProjectCategory ProjectCategory)
        {
            await _repository.AddAsync(ProjectCategory);
        }

        public async Task UpdateAsync(ProjectCategory ProjectCategory)
        {
            await _repository.UpdateAsync(ProjectCategory);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
