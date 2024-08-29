using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IEvaluationCriteriaService
    {
        Task<EvaluationCriteria> GetByIdAsync(string id);
        Task<IEnumerable<EvaluationCriteria>> GetAllAsync();
        Task AddAsync(EvaluationCriteria evaluationCriteria);
        Task UpdateAsync(EvaluationCriteria evaluationCriteria);
        Task DeleteAsync(string id);
    }


}
