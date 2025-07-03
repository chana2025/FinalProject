using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // נדרש עבור ToListAsync, FirstOrDefaultAsync

namespace Repository.Repositories
{
    public class DietTypeRepository : IRepository<DietType>
    {
        private readonly IContext _context;

        public DietTypeRepository(IContext context)
        {
            this._context = context;
        }

        public async Task<DietType> AddItemAsync(DietType item)
        {
            await _context.DietTypes.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) 
                return; // או לזרוק חריגה אם תרצה

            _context.DietTypes.Remove(entity);
            await _context.SaveAsync();
        }

        public async Task<List<DietType>> GetAllAsync()
        {
            return await _context.DietTypes.ToListAsync();
        }

        public async Task<DietType?> GetByIdAsync(int id)
        {
            return await _context.DietTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateItemAsync(int id, DietType item)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return;

            existing.DietName = item.DietName;
            existing.Calories = item.Calories;
            existing.Carbohydrates = item.Carbohydrates;
            existing.Fat = item.Fat;
            existing.Protein = item.Protein;
            existing.SpecialComments = item.SpecialComments;
            existing.ImageUrl = item.ImageUrl;

            // נעדכן רק את TimeMealsString – זה שמישמר במסד

            // ⚠️ לא נעדכן Customers – אלא אם באמת יש צורך, וגם אז בזהירות עם Tracking
            // existing.Customers = item.Customers;

            await _context.SaveAsync();
        }
    }
}
