using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface IWeeklyLogsRepository
    {
        Task<WeeklyLogs> GetByIdAsync(string id);
        Task<IEnumerable<WeeklyLogs>> GetAllAsync();
        Task AddAsync(WeeklyLogs weeklyLogs);
        Task UpdateAsync(WeeklyLogs weeklyLogs);
        Task DeleteAsync(string id);
    }

}
