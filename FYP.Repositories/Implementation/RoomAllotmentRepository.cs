using FYP.Db;
using FYP.Entities;
using FYP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP.Repositories.Implementation
{
    public class RoomAllotmentRepository : Interfaces.IRoomAllotmentRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomAllotmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RoomAllotment> GetByIdAsync(string id)
        {
            return await _context.RoomAllotments.FindAsync(id);
        }

        public async Task<IEnumerable<RoomAllotment>> GetAllAsync()
        {
            return await _context.RoomAllotments.ToListAsync();
        }

        public async Task AddAsync(RoomAllotment roomAllotment)
        {
            _context.RoomAllotments.Add(roomAllotment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoomAllotment roomAllotment)
        {
            _context.RoomAllotments.Update(roomAllotment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var roomAllotment = await GetAllAsync();
            var RoomAllotment = roomAllotment.Where(x => x.ID.ToString() == id).FirstOrDefault();
            if (RoomAllotment != null)
            {
                _context.RoomAllotments.Remove(RoomAllotment);
                await _context.SaveChangesAsync();
            }
           
        }

      
    }

}
