using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface IChangeTitleFormRepository
    {
        Task<ChangeTitleForm> GetByIdAsync(string id);
        Task<IEnumerable<ChangeTitleForm>> GetAllAsync();
        Task AddAsync(ChangeTitleForm changeTitleForm);
        Task UpdateAsync(ChangeTitleForm changeTitleForm);
        Task DeleteAsync(string id);
    }

}
