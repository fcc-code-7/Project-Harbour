using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface ISupervisorRepository
    {
        Task<Supervisor> GetByIdAsync(string id);
        Task<IEnumerable<Supervisor>> GetAllAsync();
        Task AddAsync(Supervisor supervisor);
        Task UpdateAsync(Supervisor supervisor);
        Task DeleteAsync(string id);
    }

}
