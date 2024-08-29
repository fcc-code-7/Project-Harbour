using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface IFYPCommitteRepository
    {
        Task<FYPCommitte> GetByIdAsync(string id);
        Task<IEnumerable<FYPCommitte>> GetAllAsync();
        Task AddAsync(FYPCommitte fYPCommitte);
        Task UpdateAsync(FYPCommitte fYPCommitte);
        Task DeleteAsync(string id);
    }

}
