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
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Notifications> GetByIdAsync(string id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task<IEnumerable<Notifications>> GetAllAsync()
        {
            return await _context.Notifications.ToListAsync();
        }

        public async Task AddAsync(Notifications Notification)
        {
            _context.Notifications.Add(Notification);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notifications Notification)
        {
            _context.Notifications.Update(Notification);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var Notificationss = await GetAllAsync();
            var Notification = Notificationss.Where(x=>x.ID.ToString() == id).FirstOrDefault();
            if (Notification != null)
            {
                _context.Notifications.Remove(Notification);
                await _context.SaveChangesAsync();
            }
        }
    }

}
