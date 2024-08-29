using FYP.Entities;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IDesignationService
    {
        Task<Designation> GetByIdAsync(string id);
        Task<IEnumerable<Designation>> GetAllAsync();
        Task AddAsync(Designation Designation);
        Task UpdateAsync(Designation Designation);
        Task DeleteAsync(string id);
    }


}
