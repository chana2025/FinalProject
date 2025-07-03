using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // נדרש ל-EF Core Async extensions

namespace Repository.Repositories
{
    public class ProductForDietTypeRepository : IRepository<ProductForDietType>
    {
        private readonly IContext _context;

        public ProductForDietTypeRepository(IContext context)
        {
            _context = context;
        }

        public async Task<ProductForDietType> AddItemAsync(ProductForDietType item)
        {
            await _context.ProductForDietTypes.AddAsync(item);
            await _context.SaveAsync();
            return item;
        }

        public async Task DeleteItemAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return; // אפשר לזרוק חריגה במקום אם רוצים

            _context.ProductForDietTypes.Remove(entity);
            await _context.SaveAsync();
        }

        public async Task<List<ProductForDietType>> GetAllAsync()
        {
            return await _context.ProductForDietTypes.ToListAsync();
        }

        public async Task<ProductForDietType> GetByIdAsync(int id)
        {
            return await _context.ProductForDietTypes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateItemAsync(int id, ProductForDietType item)
        {
            var existing = await GetByIdAsync(id);
            if (existing == null) return;

            // כאן אפשר לעדכן שדות רלוונטיים, לדוגמה:
            // existing.ProdName = item.ProdName;

            await _context.SaveAsync();
        }
    }
}
