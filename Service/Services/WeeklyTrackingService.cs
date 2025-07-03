using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public class WeeklyTrackingService : IService<WeeklyTrackingDto>
    {
        private readonly IRepository<WeeklyTracking> _repository;
        private readonly IMapper _mapper;

        public WeeklyTrackingService(IRepository<WeeklyTracking> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WeeklyTrackingDto> AddItemAsync(WeeklyTrackingDto item)
        {
            var entity = _mapper.Map<WeeklyTrackingDto, WeeklyTracking>(item);
            var added = await _repository.AddItemAsync(entity);
            return _mapper.Map<WeeklyTracking, WeeklyTrackingDto>(added);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _repository.DeleteItemAsync(id);
        }

        public async Task<List<WeeklyTrackingDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<WeeklyTracking>, List<WeeklyTrackingDto>>(entities);
        }

        public async Task<WeeklyTrackingDto> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<WeeklyTracking, WeeklyTrackingDto>(entity);
        }

        public async Task UpdateItemAsync(int id, WeeklyTrackingDto item)
        {
            var entity = _mapper.Map<WeeklyTrackingDto, WeeklyTracking>(item);
            await _repository.UpdateItemAsync(id, entity);
        }
    }
}
