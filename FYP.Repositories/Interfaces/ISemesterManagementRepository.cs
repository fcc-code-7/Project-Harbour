using FYP.Entities;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface ISemesterManagementRepository
    {
        Task<SemesterManagement> GetByIdAsync(string id);
        Task<IEnumerable<SemesterManagement>> GetAllAsync();
        Task AddAsync(SemesterManagement semesterManagement);
        Task UpdateAsync(SemesterManagement semesterManagement);
        Task DeleteAsync(string id);
    }

}
