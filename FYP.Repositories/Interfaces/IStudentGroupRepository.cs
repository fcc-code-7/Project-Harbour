using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Interfaces
{
    public interface IStudentGroupRepository
    {
        Task<StudentGroup> GetByIdAsync(string id);
        Task<IEnumerable<StudentGroup>> GetAllAsync();
        Task AddAsync(StudentGroup studentGroup);
        Task UpdateAsync(StudentGroup studentGroup);
        Task DeleteAsync(string id);
    }

}
