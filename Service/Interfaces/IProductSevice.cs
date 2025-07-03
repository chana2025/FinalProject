using Common.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> AddItemAsync(ProductDto dto);
        Task DeleteItemAsync(int id);
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task UpdateItemAsync(int id, ProductDto dto);
        Task<List<ProductDto>> GetByNameAsync(string name);
        Task<int> SaveProductsFromApiAsync(List<ProductDto> productsFromApi);
    }
}
