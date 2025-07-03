using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class CustomerService : IService<CustomerDto>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<Product> _productRepository; // הוספתי
        private readonly IMapper _mapper;

        public CustomerService(IRepository<Customer> repository, IRepository<Product> productRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository; // הוספתי
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> AddItemAsync(CustomerDto item)
        {
            var customer = _mapper.Map<Customer>(item);

            customer.FoodPreferences = new List<CustomerFoodPreference>();

            var validProductIds = (await _productRepository.GetAllAsync()).Select(p => p.ProductId).ToHashSet();

            if (item.LikedProductIds != null)
            {
                foreach (var likedId in item.LikedProductIds)
                {
                    if (validProductIds.Contains(likedId))
                    {
                        customer.FoodPreferences.Add(new CustomerFoodPreference
                        {
                            ProductId = likedId,
                            IsLiked = true
                        });
                    }
                }
            }

            if (item.DislikedProductIds != null)
            {
                foreach (var dislikedId in item.DislikedProductIds)
                {
                    if (validProductIds.Contains(dislikedId))
                    {
                        customer.FoodPreferences.Add(new CustomerFoodPreference
                        {
                            ProductId = dislikedId,
                            IsLiked = false
                        });
                    }
                }
            }

            var added = await _repository.AddItemAsync(customer);
            return _mapper.Map<CustomerDto>(added);
        }

        public async Task UpdateItemAsync(int id, CustomerDto item)
        {
            var customer = _mapper.Map<Customer>(item);

            customer.FoodPreferences = new List<CustomerFoodPreference>();

            var validProductIds = (await _productRepository.GetAllAsync()).Select(p => p.ProductId).ToHashSet();

            if (item.LikedProductIds != null)
            {
                foreach (var likedId in item.LikedProductIds)
                {
                    if (validProductIds.Contains(likedId))
                    {
                        customer.FoodPreferences.Add(new CustomerFoodPreference
                        {
                            ProductId = likedId,
                            IsLiked = true
                        });
                    }
                }
            }

            if (item.DislikedProductIds != null)
            {
                foreach (var dislikedId in item.DislikedProductIds)
                {
                    if (validProductIds.Contains(dislikedId))
                    {
                        customer.FoodPreferences.Add(new CustomerFoodPreference
                        {
                            ProductId = dislikedId,
                            IsLiked = false
                        });
                    }
                }
            }

            await _repository.UpdateItemAsync(id, customer);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _repository.DeleteItemAsync(id);
        }
    }
}
