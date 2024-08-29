using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IProjectCategoryService
    {
        Task<ProjectCategory> GetByIdAsync(string id);
        Task<IEnumerable<ProjectCategory>> GetAllAsync();
        Task AddAsync(ProjectCategory projectCategory);
        Task UpdateAsync(ProjectCategory projectCategory);
        Task DeleteAsync(string id);
    }


}
