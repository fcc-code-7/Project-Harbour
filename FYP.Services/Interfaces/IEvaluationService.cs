using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IEvaluationService
    {
        Task<Evaluation> GetByIdAsync(string id);
        Task<IEnumerable<Evaluation>> GetAllAsync();
        Task AddAsync(Evaluation evaluation);
        Task UpdateAsync(Evaluation evaluation);
        Task DeleteAsync(string id);
    }


}
