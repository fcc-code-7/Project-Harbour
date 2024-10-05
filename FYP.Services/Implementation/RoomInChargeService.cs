using FYP.Db;
using FYP.Entities;
using FYP.Models;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Services.Implementation
{
    public class RoomInChargeService : IRoomInChargeService
    {
        private readonly Repositories.Interfaces.IRoomInCharge _repository;

        public RoomInChargeService(Repositories.Interfaces.IRoomInCharge repository)
        {
            _repository = repository;
        }

        public async Task<RoomInCharge> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<RoomInCharge>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(RoomInCharge RoomInCharge)
        {
            await _repository.AddAsync(RoomInCharge);
        }

        public async Task UpdateAsync(RoomInCharge RoomInCharge)
        {
            await _repository.UpdateAsync(RoomInCharge);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }


}
