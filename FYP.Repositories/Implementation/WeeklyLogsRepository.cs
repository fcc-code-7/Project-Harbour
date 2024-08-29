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
    public class WeeklyLogsRepository : IWeeklyLogsRepository
    {
        private readonly ApplicationDbContext _context;

        public WeeklyLogsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WeeklyLogs> GetByIdAsync(string id)
        {
            return await _context.WeeklyLogs.FindAsync(id);
        }

        public async Task<IEnumerable<WeeklyLogs>> GetAllAsync()
        {
            return await _context.WeeklyLogs.ToListAsync();
        }

        public async Task AddAsync(WeeklyLogs WeeklyLogs)
        {
            _context.WeeklyLogs.Add(WeeklyLogs);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WeeklyLogs WeeklyLogs)
        {
            _context.WeeklyLogs.Update(WeeklyLogs);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var studentGroup = await GetByIdAsync(id);
            if (studentGroup != null)
            {
                _context.WeeklyLogs.Remove(studentGroup);
                await _context.SaveChangesAsync();
            }
        }
    }

}
