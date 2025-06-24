using System;
using System.Collections.Generic;
using System.Linq;
using Common.Dto;
using Service.Interfaces; // ודא שזה מיובא
using Repository.Entities;
using Repository.Interfaces; // ודא שזה מיובא

// ✅ שינוי: מממש את IProductService במקום IService<ProductDto>
namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public ProductDto AddItem(ProductDto dto)
        {
            var entity = MapToEntity(dto);
            var added = _repository.AddItem(entity);
            return MapToDto(added);
        }

        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }

        public List<ProductDto> GetAll()
        {
            return _repository.GetAll()
                .Select(MapToDto)
                .ToList();
        }

        public ProductDto GetById(int id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : MapToDto(entity);
        }

        public List<ProductDto> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<ProductDto>();
            }

            return _repository.GetAll()
                              .Where(p => p.Name != null && p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                              .Select(MapToDto)
                              .ToList();
        }

        public void UpdateItem(int id, ProductDto dto)
        {
            var entity = MapToEntity(dto);
            _repository.UpdateItem(id, entity);
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

        // כאן העדכון: הפוך את המתודה לאסינכרונית והשתמש ב-Task<int> במקום int
        public async Task<int> SaveProductsFromApi(List<ProductDto> productsFromApi)
        {
            int count = 0;

            // אם _repository.GetAll() הוא סינכרוני, אפשר להשאיר, אחרת תתאים לאסינכרוני
            var existingProducts = _repository.GetAll().Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var dto in productsFromApi)
            {
                if (!existingProducts.Contains(dto.Name))
                {
                    var entity = MapToEntity(dto);
                    _repository.AddItem(entity);
                    count++;
                }
            }

            // במידה ויש כאן פעולה אסינכרונית (כגון שמירת שינויים), הוסף await כאן
            // await _repository.SaveChangesAsync();

            return await Task.FromResult(count); // החזרה אסינכרונית פשוטה
        }
    }
}
