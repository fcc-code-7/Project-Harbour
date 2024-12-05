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
    public class StudentGroupRepository : IStudentGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentGroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StudentGroup> GetByIdAsync(string id)
        {
            return await _context.StudentGroups.FindAsync(id);
        }

        public async Task<IEnumerable<StudentGroup>> GetAllAsync()
        {
            return await _context.StudentGroups.ToListAsync();
        }

        public async Task AddAsync(StudentGroup studentGroup)
        {
            _context.StudentGroups.Add(studentGroup);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentGroup studentGroup)
        {
            _context.StudentGroups.Update(studentGroup);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var group = await GetAllAsync();
            var Groups = group.Where(x => x.ID.ToString() == id).FirstOrDefault();
            if (Groups != null)
            {
                _context.StudentGroups.Remove(Groups);
                await _context.SaveChangesAsync();
            }
        }
    }

}
