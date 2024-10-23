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
    public class RoomsRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room> GetByIdAsync(string id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task AddAsync(Room Room)
        {
            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room Room)
        {
            _context.Rooms.Update(Room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var Rooms = await GetAllAsync();
            var Room = Rooms.Where(x=>x.ID.ToString() == id).FirstOrDefault();
            if (Room != null)
            {
                _context.Rooms.Remove(Room);
                await _context.SaveChangesAsync();
            }
        }
    }

}
