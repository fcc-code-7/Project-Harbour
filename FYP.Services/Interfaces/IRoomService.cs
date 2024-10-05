using FYP.Entities;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IRoomService
    {
        Task<Room> GetByIdAsync(string id);
        Task<IEnumerable<Room>> GetAllAsync();
        Task AddAsync(Room Room);
        Task UpdateAsync(Room Room);
        Task DeleteAsync(string id);
    }


}
