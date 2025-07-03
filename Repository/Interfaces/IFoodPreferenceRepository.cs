using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IFoodPreferenceRepository
    {
        Task AddItemAsync(CustomerFoodPreference pref);
        Task DeleteByCustomerIdAsync(int customerId);
        Task<List<CustomerFoodPreference>> GetByCustomerIdAsync(int customerId);
    }
}
