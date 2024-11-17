using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface IRoomAllotmentRepository
    {
        Task<RoomAllotment> GetByIdAsync(string id);
        Task<IEnumerable<RoomAllotment>> GetAllAsync();
        Task AddAsync(RoomAllotment roomAllotment);
        Task UpdateAsync(RoomAllotment roomAllotment);
        Task DeleteAsync(string id);
    }

}
