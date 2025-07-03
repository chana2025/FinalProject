using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly IContext _context;

        public CustomerRepository(IContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddItemAsync(Customer item)
        {
            await _context.Customers.AddAsync(item);
            await _context.SaveAsync(); // חשוב!
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveAsync(); // חשוב!
            }
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateItemAsync(int id, Customer item)
        {
            var existing = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (existing != null)
            {
                // עדכון שדות בסיסיים
                existing.FullName = item.FullName;
                existing.Email = item.Email;
                existing.Password = item.Password;
                existing.Phone = item.Phone;
                existing.Role = item.Role;
                existing.Height = item.Height;
                existing.Weight = item.Weight;
                existing.ImagePath = item.ImagePath;
                existing.ImageUrl = item.ImageUrl;
                existing.Gender = item.Gender;
                existing.DietId = item.DietId;

                // עדכון FoodPreferences:
                existing.FoodPreferences.Clear();

                if (item.FoodPreferences != null)
                {
                    foreach (var pref in item.FoodPreferences)
                    {
                        existing.FoodPreferences.Add(pref);
                    }
                }

                await _context.SaveAsync(); // שמירה במסד
            }
        }
    }
}
