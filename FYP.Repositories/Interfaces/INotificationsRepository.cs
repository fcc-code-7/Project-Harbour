using FYP.Entities;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface INotificationsRepository
    {
        Task<Notifications> GetByIdAsync(string id);
        Task<IEnumerable<Notifications>> GetAllAsync();
        Task AddAsync(Notifications Notification);
        Task UpdateAsync(Notifications Notification);
        Task DeleteAsync(string id);
    }

}
