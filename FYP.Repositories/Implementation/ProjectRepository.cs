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
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project> GetByIdAsync(string id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task AddAsync(Project Project)
        {
            _context.Projects.Add(Project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project Project)
        {
            _context.Projects.Update(Project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var Project = await GetByIdAsync(id);
            if (Project != null)
            {
                _context.Projects.Remove(Project);
                await _context.SaveChangesAsync();
            }
        }
    }

}
