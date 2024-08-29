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
    public class WeeklyLogsService : IWeeklyLogsService
    {
        private readonly IWeeklyLogsRepository _repository;

        public WeeklyLogsService(IWeeklyLogsRepository repository)
        {
            _repository = repository;
        }

        public async Task<WeeklyLogs> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<WeeklyLogs>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(WeeklyLogs WeeklyLogs)
        {
            await _repository.AddAsync(WeeklyLogs);
        }

        public async Task UpdateAsync(WeeklyLogs WeeklyLogs)
        {
            await _repository.UpdateAsync(WeeklyLogs);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
