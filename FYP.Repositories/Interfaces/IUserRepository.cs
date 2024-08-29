using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetByIdAsync(string id);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task DeleteAsync(string id);
    }

}
