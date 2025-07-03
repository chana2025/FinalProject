using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Service
{
    public class DietTypeService : IService<DietDto>
    {
        private readonly IRepository<DietType> _repository;
        private readonly IMapper _mapper;

        public DietTypeService(IRepository<DietType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DietDto> AddItemAsync(DietDto item)
        {
            string savedImagePath = null;

            if (item.fileImage != null && item.fileImage.Length > 0)
            {
                var fileName = Path.GetFileName(item.fileImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "MyProject", "Images");
                Directory.CreateDirectory(directory);
                savedImagePath = Path.Combine(directory, fileName);

                using (var stream = new FileStream(savedImagePath, FileMode.Create))
                {
                    await item.fileImage.CopyToAsync(stream);
                }

                item.ImageUrl = Path.Combine("MyProject", "Images", fileName);
            }

            var dietEntity = _mapper.Map<DietDto, DietType>(item);
            var addedEntity = await _repository.AddItemAsync(dietEntity);
            return _mapper.Map<DietType, DietDto>(addedEntity);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _repository.DeleteItemAsync(id);
        }

        public async Task<List<DietDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<DietType>, List<DietDto>>(entities);
        }

        public async Task<DietDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<DietType, DietDto>(entity);
        }

        public async Task UpdateItemAsync(int id, DietDto item)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            existing.DietName = item.DietName;
            existing.Calories = item.Calories;
            existing.Carbohydrates = item.Carbohydrates;
            existing.Fat = item.Fat;
            existing.Protein = item.Protein;
            existing.SpecialComments = item.SpecialComments;
            existing.ImageUrl = item.ImageUrl;

            if (item.fileImage != null && item.fileImage.Length > 0)
            {
                var fileName = Path.GetFileName(item.fileImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "MyProject", "Images");
                Directory.CreateDirectory(directory);

                var savedImagePath = Path.Combine(directory, fileName);
                using (var stream = new FileStream(savedImagePath, FileMode.Create))
                {
                    await item.fileImage.CopyToAsync(stream);
                }

                existing.ImageUrl = Path.Combine("MyProject", "Images", fileName);
            }

            await _repository.UpdateItemAsync(id, existing);
        }
    }
}
