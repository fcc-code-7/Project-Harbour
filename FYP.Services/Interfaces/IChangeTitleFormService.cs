using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IChangeTitleFormService
    {
        Task<ChangeTitleForm> GetByIdAsync(string id);
        Task<IEnumerable<ChangeTitleForm>> GetAllAsync();
        Task AddAsync(ChangeTitleForm changeTitleForm);
        Task UpdateAsync(ChangeTitleForm changeTitleForm);
        Task DeleteAsync(string id);
    }


}
