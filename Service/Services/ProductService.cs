using System;
using System.Collections.Generic;
using System.Linq;
using Common.Dto;
using Service.Interfaces; // ודא שזה מיובא
using Repository.Entities;
using Repository.Interfaces; // ודא שזה מיובא

namespace Service.Services
{
    // ✅ שינוי: מממש את IProductService במקום IService<ProductDto>
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
            // ✅ הגנה: החזר null אם entity הוא null כדי למנוע NullReferenceException
            return entity == null ? null : MapToDto(entity);
        }

        // ✅ הוספה: מימוש מתודת החיפוש לפי שם
        public List<ProductDto> GetByName(string name)
        {
            // ודא שהשם אינו ריק או null
            if (string.IsNullOrWhiteSpace(name))
            {
                // אם מחרוזת החיפוש ריקה, נחזיר רשימה ריקה
                return new List<ProductDto>();
            }

            // חיפוש מוצרים שהשם שלהם מכיל את מחרוזת החיפוש (לא תלוי רישיות)
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
            // ✅ הגנה: החזר null אם product הוא null
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
            // ✅ הגנה: החזר null אם dto הוא null
            if (dto == null) return null;
            return new Product
            {
                ProductId = dto.ProductId,
                Name = dto.Name ?? "", // ודא ש-Name אינו null
                Calories = dto.Calories,
                Protein = dto.Protein,
                Fat = dto.Fat,
                Carbohydrates = dto.Carbohydrates,
                SourceApi = dto.SourceApi
            };
        }
    }
}