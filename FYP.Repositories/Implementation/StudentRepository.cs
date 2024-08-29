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
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Student> GetByIdAsync(string id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task AddAsync(Student Student)
        {
            _context.Students.Add(Student);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Student Student)
        {
            _context.Students.Update(Student);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var Student = await GetByIdAsync(id);
            if (Student != null)
            {
                _context.Students.Remove(Student);
                await _context.SaveChangesAsync();
            }
        }
    }

}
