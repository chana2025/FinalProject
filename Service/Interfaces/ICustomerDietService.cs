using Common.Dto;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICustomerDietService
    {
        Task AddAsync(CustomerDietDto dto);
        Task<DietDto?> GetDietByCustomerIdAsync(int customerId);

    }
}
