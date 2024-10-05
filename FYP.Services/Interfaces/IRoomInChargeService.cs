using FYP.Entities;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IRoomInChargeService
    {
        Task<RoomInCharge> GetByIdAsync(string id);
        Task<IEnumerable<RoomInCharge>> GetAllAsync();
        Task AddAsync(RoomInCharge RoomInCharge);
        Task UpdateAsync(RoomInCharge RoomInCharge);
        Task DeleteAsync(string id);
    }


}
