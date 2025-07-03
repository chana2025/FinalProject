using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddItemAsync(T item);
        Task DeleteItemAsync(int id);
        Task UpdateItemAsync(int id, T item);
    }
}
