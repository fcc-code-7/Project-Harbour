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

namespace FYP.Repositories.Implementation
{
    public class RoomInChargeRepository : IRoomInCharge
    {
        private readonly ApplicationDbContext _context;

        public RoomInChargeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoomInCharge> GetByIdAsync(string id)
        {
            return await _context.RoomInCharge.FindAsync(id);
        }

        public async Task<IEnumerable<RoomInCharge>> GetAllAsync()
        {
            return await _context.RoomInCharge.ToListAsync();
        }

        public async Task AddAsync(RoomInCharge RoomInCharge)
        {
            _context.RoomInCharge.Add(RoomInCharge);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoomInCharge RoomInCharge)
        {
            _context.RoomInCharge.Update(RoomInCharge);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var RoomInCharge = await GetAllAsync();
            var roomincharge = RoomInCharge.Where(x => x.ID.ToString() == id).FirstOrDefault();
            if (roomincharge != null)
            {
                _context.RoomInCharge.Remove(roomincharge);
                await _context.SaveChangesAsync();
            }
            
        }
    }

}
