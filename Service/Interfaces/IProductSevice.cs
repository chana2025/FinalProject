using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using System.Collections.Generic;
namespace Service.Interfaces
{
    public interface IProductService : IService<ProductDto> // אם IService<T> הוא גנרי כללי
    {
        List<ProductDto> GetByName(string name);
    }
}
