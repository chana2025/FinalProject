using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // כדי להשתמש ב-ToListAsync וכו'

public class ProductRepository : IRepository<Product>
{
    private readonly IContext _context;
    public ProductRepository(IContext context) => _context = context;

    public async Task<Product> AddItemAsync(Product item)
    {
        await _context.Products.AddAsync(item);
        await _context.SaveAsync();
        return item;
    }

    public async Task DeleteItemAsync(int id)
    {
        var product = await GetByIdAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveAsync();
        }
    }

    public async Task<List<Product>> GetAllAsync() => await _context.Products.ToListAsync();

    public async Task<Product> GetByIdAsync(int id) =>
        await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

    public async Task UpdateItemAsync(int id, Product item)
    {
        var p = await GetByIdAsync(id);
        if (p != null)
        {
            p.Name = item.Name;
            p.Calories = item.Calories;
            p.Protein = item.Protein;
            p.Fat = item.Fat;
            p.Carbohydrates = item.Carbohydrates;
            p.SourceApi = item.SourceApi;
            await _context.SaveAsync();
        }
    }
}
