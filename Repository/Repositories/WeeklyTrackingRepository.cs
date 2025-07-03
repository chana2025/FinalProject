using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class WeeklyTrackingRepository : IRepository<WeeklyTracking>
    {
        private readonly IContext _context;

        public WeeklyTrackingRepository(IContext context)
        {
            _context = context;
        }

        public async Task<WeeklyTracking> AddItemAsync(WeeklyTracking item)
        {
            await _context.WeeklyTrackings.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return;

            _context.WeeklyTrackings.Remove(entity);
            await _context.SaveAsync();
        }

        public async Task<List<WeeklyTracking>> GetAllAsync()
        {
            return await _context.WeeklyTrackings.ToListAsync();
        }

        public async Task<WeeklyTracking> GetByIdAsync(int id)
        {
            return await _context.WeeklyTrackings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateItemAsync(int id, WeeklyTracking item)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return;

            existing.CustId = item.CustId;
            existing.WeekDate = item.WeekDate;
            existing.IsPassCalories = item.IsPassCalories;
            existing.UpdatdedWieght = item.UpdatdedWieght;
            // יש לוודא שעדכנת כאן את כל השדות הרלוונטיים

            await _context.SaveAsync();
        }
    }
}
