using FYP.Entities;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IChangeSupervisorFormService
    {
        Task<ChangeSupervisorForm> GetByIdAsync(string id);
        Task<IEnumerable<ChangeSupervisorForm>> GetAllAsync();
        Task AddAsync(ChangeSupervisorForm ChangeSupervisorForm);
        Task UpdateAsync(ChangeSupervisorForm ChangeSupervisorForm);
        Task DeleteAsync(string id);
    }


}
