using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Dto;
using Service.Interfaces;
using Repository.Entities;
using Repository.Interfaces;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<ProductDto> AddItemAsync(ProductDto dto)
        {
            var entity = MapToEntity(dto);
            var added = await _repository.AddItemAsync(entity);
            return MapToDto(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _repository.DeleteItemAsync(id);
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MapToDto).ToList();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }

        public async Task<List<ProductDto>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<ProductDto>();
            }

            var all = await _repository.GetAllAsync();
            return all
                .Where(p => p.Name != null && p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(MapToDto)
                .ToList();
        }

        public async Task UpdateItemAsync(int id, ProductDto dto)
        {
            var entity = MapToEntity(dto);
            await _repository.UpdateItemAsync(id, entity);
        }

        private ProductDto MapToDto(Product product)
        {
            if (product == null) return null;
            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Calories = product.Calories,
                Protein = product.Protein,
                Fat = product.Fat,
                Carbohydrates = product.Carbohydrates,
                SourceApi = product.SourceApi
            };
        }

        private Product MapToEntity(ProductDto dto)
        {
            if (dto == null) return null;
            return new Product
            {
                ProductId = dto.ProductId,
                Name = dto.Name ?? "",
                Calories = dto.Calories,
                Protein = dto.Protein,
                Fat = dto.Fat,
                Carbohydrates = dto.Carbohydrates,
                SourceApi = dto.SourceApi
            };
        }

        public async Task<int> SaveProductsFromApiAsync(List<ProductDto> productsFromApi)
        {
            int count = 0;
            var existingProducts = (await _repository.GetAllAsync())
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var dto in productsFromApi)
            {
                if (!existingProducts.Contains(dto.Name))
                {
                    var entity = MapToEntity(dto);
                    await _repository.AddItemAsync(entity);
                    count++;
                }
            }

            // במידה ויש SaveChangesAsync, הוסף כאן:
            // await _repository.SaveChangesAsync();

            return count;
        }
    }
}
