using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IRoomAllotmentService
    {
        Task<RoomAllotment> GetByIdAsync(string id);
        Task<IEnumerable<RoomAllotment>> GetAllAsync();
        Task AddAsync(RoomAllotment roomAllotment);
        Task UpdateAsync(RoomAllotment roomAllotment);
        Task DeleteAsync(string id);
    }


}
