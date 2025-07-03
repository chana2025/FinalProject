using Common.Dto;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.Interfaces;
using Repository.Entities;

namespace Service.Services
{
    public class FoodPreferenceService : IFoodPreferenceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodPreferenceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SaveUserPreferencesAsync(FoodPreferencesDto dto, int userId)
        {
            await _unitOfWork.FoodPreferenceRepository.DeleteByCustomerIdAsync(userId);

            if (dto.LikedProductIds != null)
            {
                foreach (var productId in dto.LikedProductIds)
                {
                    var pref = new CustomerFoodPreference
                    {
                        CustomerId = userId,
                        ProductId = productId,
                        IsLiked = true
                    };
                    await _unitOfWork.FoodPreferenceRepository.AddItemAsync(pref);
                }
            }

            if (dto.DislikedProductIds != null)
            {
                foreach (var productId in dto.DislikedProductIds)
                {
                    var pref = new CustomerFoodPreference
                    {
                        CustomerId = userId,
                        ProductId = productId,
                        IsLiked = false
                    };
                    await _unitOfWork.FoodPreferenceRepository.AddItemAsync(pref);
                }
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<FoodPreferencesDto> GetUserPreferencesAsync(int userId)
        {
            var preferences = await _unitOfWork.FoodPreferenceRepository.GetByCustomerIdAsync(userId);

            if (preferences == null || !preferences.Any())
            {
                return new FoodPreferencesDto
                {
                    CustomerId = userId,
                    LikedProductIds = new List<int>(),
                    DislikedProductIds = new List<int>()
                };
            }

            var liked = preferences.Where(p => p.IsLiked).Select(p => p.ProductId).ToList();
            var disliked = preferences.Where(p => !p.IsLiked).Select(p => p.ProductId).ToList();

            return new FoodPreferencesDto
            {
                CustomerId = userId,
                LikedProductIds = liked,
                DislikedProductIds = disliked
            };
        }

        public async Task ClearPreferencesAsync(int userId)
        {
            await _unitOfWork.FoodPreferenceRepository.DeleteByCustomerIdAsync(userId);
            await _unitOfWork.SaveAsync();
        }
    }
}
