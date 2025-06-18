using Repository.Entities;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly IContext _context;

        public CustomerRepository(IContext context)
        {
            _context = context;
        }

        public Customer AddItem(Customer item)
        {
            _context.Customers.Add(item);
            _context.Save(); // חשוב!
            return item;
        }

        public void DeleteItem(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.Save(); // חשוב!
            }
        }

        public List<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public Customer GetById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id)!;
        }

        public void UpdateItem(int id, Customer item)
        {
            var existing = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (existing != null)
            {
                // עדכון שדות בלבד - בלי Attach או Update
                existing.FullName = item.FullName;
                existing.Email = item.Email;
                existing.Password = item.Password;
                existing.Phone = item.Phone;
                existing.Role = item.Role;
                existing.Height = item.Height;
                existing.Weight = item.Weight;
                existing.ImagePath = item.ImagePath;

                _context.Save(); // שמירה רק פעם אחת
            }
        }


    }
}
