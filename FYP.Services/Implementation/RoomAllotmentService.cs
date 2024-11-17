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
    public class RoomAllotmentService : IRoomAllotmentService
    {
        private readonly IRoomAllotmentRepository _repository;

        public RoomAllotmentService(IRoomAllotmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoomAllotment> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RoomAllotment>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(RoomAllotment roomAllotment)
        {
            await _repository.AddAsync(roomAllotment);
        }

        public async Task UpdateAsync(RoomAllotment roomAllotment)
        {
            await _repository.UpdateAsync(roomAllotment);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
