using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System.Threading.Tasks;

namespace Service.Services
{
    public class CustomerDietService : ICustomerDietService
    {
        private readonly ICustomerDietRepository _repository;

        public CustomerDietService(ICustomerDietRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(CustomerDietDto dto)
        {
            var existingDiets = await _repository.GetAllByCustomerIdAsync(dto.CustomerId);

            if (existingDiets.Any())
            {
                foreach (var diet in existingDiets)
                {
                    await _repository.RemoveAsync(diet);
                }
                await _repository.SaveChangesAsync();
            }

            var newEntity = new CustomerDiet
            {
                CustomerId = dto.CustomerId,
                DietId = dto.DietId
            };

            await _repository.AddAsync(newEntity);
            await _repository.SaveChangesAsync();
        }

        public async Task<DietDto?> GetDietByCustomerIdAsync(int customerId)
        {
            var entity = await _repository.GetByCustomerIdAsync(customerId);
            if (entity == null) return null;

            return new DietDto
            {
                DietId = entity.Diet.Id,
                DietName = entity.Diet.DietName,
                Calories = entity.Diet.Calories,
                Protein = entity.Diet.Protein,
                Fat = entity.Diet.Fat,
                Carbohydrates = entity.Diet.Carbohydrates,
                SpecialComments = entity.Diet.SpecialComments,
                ImageUrl = entity.Diet.ImageUrl
            };
        }
    }
}
