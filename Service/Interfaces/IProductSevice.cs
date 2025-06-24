using Common.Dto;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IProductService
    {
        ProductDto AddItem(ProductDto dto);
        void DeleteItem(int id);
        List<ProductDto> GetAll();
        ProductDto GetById(int id);
        void UpdateItem(int id, ProductDto dto);
        List<ProductDto> GetByName(string name);

        Task<int> SaveProductsFromApi(List<ProductDto> productsFromApi);
    }
}
