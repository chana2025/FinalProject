using Common.Dto;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IFoodPreferenceService
    {
        Task SaveUserPreferencesAsync(FoodPreferencesDto dto, int userId);
        Task<FoodPreferencesDto> GetUserPreferencesAsync(int userId);
        Task ClearPreferencesAsync(int userId);
    }
}
