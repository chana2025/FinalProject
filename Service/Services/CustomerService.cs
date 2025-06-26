using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class CustomerService : IService<CustomerDto>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public CustomerService(IRepository<Customer> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public CustomerDto GetById(int id)
        {
            var customer = _repository.GetById(id);
            return _mapper.Map<CustomerDto>(customer);
        }

        public List<CustomerDto> GetAll()
        {
            var customers = _repository.GetAll();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public CustomerDto AddItem(CustomerDto item)
        {
            var customer = _mapper.Map<Customer>(item);

            // ניהול העדפות אוכל - מחיקת העדפות ישנות והוספת חדשות
            customer.FoodPreferences = new List<CustomerFoodPreference>();

            if (item.LikedProductIds != null)
            {
                foreach (var likedId in item.LikedProductIds)
                {
                    customer.FoodPreferences.Add(new CustomerFoodPreference
                    {
                        ProductId = likedId,
                        IsLiked = true
                    });
                }
            }

            if (item.DislikedProductIds != null)
            {
                foreach (var dislikedId in item.DislikedProductIds)
                {
                    customer.FoodPreferences.Add(new CustomerFoodPreference
                    {
                        ProductId = dislikedId,
                        IsLiked = false
                    });
                }
            }

            var added = _repository.AddItem(customer);
            return _mapper.Map<CustomerDto>(added);
        }

        public void UpdateItem(int id, CustomerDto item)
        {
            var customer = _mapper.Map<Customer>(item);

            // ניהול עדכוני העדפות אוכל
            customer.FoodPreferences = new List<CustomerFoodPreference>();

            if (item.LikedProductIds != null)
            {
                foreach (var likedId in item.LikedProductIds)
                {
                    customer.FoodPreferences.Add(new CustomerFoodPreference
                    {
                        ProductId = likedId,
                        IsLiked = true
                    });
                }
            }

            if (item.DislikedProductIds != null)
            {
                foreach (var dislikedId in item.DislikedProductIds)
                {
                    customer.FoodPreferences.Add(new CustomerFoodPreference
                    {
                        ProductId = dislikedId,
                        IsLiked = false
                    });
                }
            }

            _repository.UpdateItem(id, customer);
        }

        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }
    }
}
