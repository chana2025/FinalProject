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
            var added = _repository.AddItem(customer);
            return _mapper.Map<CustomerDto>(added);
        }

        public void DeleteItem(int id)
        {
            _repository.DeleteItem(id);
        }

        public void UpdateItem(int id, CustomerDto item)
        {
            var customer = _mapper.Map<Customer>(item);
            _repository.UpdateItem(id, customer);
        }
    }

}