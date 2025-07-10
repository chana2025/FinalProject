using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories
{
    public class CustomerDietRepository : ICustomerDietRepository
    {
        private readonly IContext _context;

        public CustomerDietRepository(IContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CustomerDiet entity)
        {
            await _context.CustomerDiet.AddAsync(entity);
            await _context.SaveAsync();
        }

        public async Task<CustomerDiet?> GetByCustomerIdAsync(int customerId)
        {
            return await _context.CustomerDiet
                .Include(cd => cd.Diet)
                .Include(cd => cd.Customer)
                .FirstOrDefaultAsync(cd => cd.CustomerId == customerId);
        }

        public async Task<CustomerDiet?> GetByCustomerIdAndDietIdAsync(int customerId, int dietId)
        {
            return await _context.CustomerDiet
                .FirstOrDefaultAsync(cd => cd.CustomerId == customerId && cd.DietId == dietId);
        }

        public async Task<List<CustomerDiet>> GetAllByCustomerIdAsync(int customerId)
        {
            return await _context.CustomerDiet
                .Where(cd => cd.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task RemoveAsync(CustomerDiet entity)
        {
            _context.CustomerDiet.Remove(entity);
            await Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveAsync();
        }
    }
}
