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
    public class ProjectCategoryRepository : IProjectCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectCategory> GetByIdAsync(string id)
        {
            return await _context.ProjectCategories.FindAsync(id);
        }

        public async Task<IEnumerable<ProjectCategory>> GetAllAsync()
        {
            return await _context.ProjectCategories.ToListAsync();
        }

        public async Task AddAsync(ProjectCategory ProjectCategory)
        {
            _context.ProjectCategories.Add(ProjectCategory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectCategory ProjectCategory)
        {
            _context.ProjectCategories.Update(ProjectCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var ProjectCategory = await GetByIdAsync(id);
            if (ProjectCategory != null)
            {
                _context.ProjectCategories.Remove(ProjectCategory);
                await _context.SaveChangesAsync();
            }
        }
    }

}
