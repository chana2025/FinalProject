using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // בשביל ToListAsync

namespace Repository.Repositories
{
    public class FoodPreferenceRepository : IFoodPreferenceRepository
    {
        private readonly IContext _context;

        public FoodPreferenceRepository(IContext context)
        {
            _context = context;
        }

        public async Task AddItemAsync(CustomerFoodPreference pref)
        {
            await _context.CustomerFoodPreferences.AddAsync(pref);
            await _context.SaveAsync();
        }

        public async Task DeleteByCustomerIdAsync(int customerId)
        {
            var prefs = await _context.CustomerFoodPreferences
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();

            _context.CustomerFoodPreferences.RemoveRange(prefs);
            await _context.SaveAsync();
        }

        public async Task<List<CustomerFoodPreference>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.CustomerFoodPreferences
                .Where(p => p.CustomerId == customerId)
                .ToListAsync();
        }
    }
}
