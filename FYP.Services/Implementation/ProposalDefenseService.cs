using FYP.Db;
using FYP.Entities;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services.Implementation
{
    public class ProposalDefenseService : IProposalDefenseService
    {
        private readonly IProposalDefenseRepository _repository;

        public ProposalDefenseService(IProposalDefenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProposalDefense> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ProposalDefense>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(ProposalDefense ProposalDefense)
        {
            await _repository.AddAsync(ProposalDefense);
        }

        public async Task UpdateAsync(ProposalDefense ProposalDefense)
        {
            await _repository.UpdateAsync(ProposalDefense);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
