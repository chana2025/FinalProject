using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ICustomerDietRepository
    {
        Task AddAsync(CustomerDiet entity);
        Task<CustomerDiet?> GetByCustomerIdAsync(int customerId);
        Task<CustomerDiet?> GetByCustomerIdAndDietIdAsync(int customerId, int dietId);
        Task<List<CustomerDiet>> GetAllByCustomerIdAsync(int customerId);
        Task RemoveAsync(CustomerDiet entity);
        Task SaveChangesAsync();
    }
}
