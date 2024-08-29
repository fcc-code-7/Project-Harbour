using FYP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services
{
    public interface IProposalDefenseService
    {
        Task<ProposalDefense> GetByIdAsync(string id);
        Task<IEnumerable<ProposalDefense>> GetAllAsync();
        Task AddAsync(ProposalDefense proposalDefense);
        Task UpdateAsync(ProposalDefense proposalDefense);
        Task DeleteAsync(string id);
    }


}
