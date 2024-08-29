using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IWeeklyLogsService
    {

        Task<WeeklyLogs> GetByIdAsync(string id);
        Task<IEnumerable<WeeklyLogs>> GetAllAsync();
        Task AddAsync(WeeklyLogs weeklyLogs);
        Task UpdateAsync(WeeklyLogs weeklyLogs);
        Task DeleteAsync(string id);
    }


}
