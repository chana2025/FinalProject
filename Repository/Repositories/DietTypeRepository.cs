using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class DietTypeRepository : IRepository<DietType>
    {
        private readonly IContext _context;

        public DietTypeRepository(IContext context)
        {
            this._context = context;
        }
        public DietType AddItem(DietType item)
        {
            this._context.DietTypes.Add(item);
            this._context.Save();
            return item;
        }

        public void DeleteItem(int id)
        {
            this._context.DietTypes.Remove(GetById(id));
            this._context.Save();
        }

        public List<DietType> GetAll()
        {
            return _context.DietTypes.ToList();
        }

        public DietType GetById(int id)
        {
            return _context.DietTypes.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void UpdateItem(int id, DietType item)
        {
            var existing = GetById(id);
            if (existing == null) return;

            existing.DietName = item.DietName;
            existing.Calories = item.Calories;
            existing.Carbohydrates = item.Carbohydrates;
            existing.Fat = item.Fat;
            existing.Protein = item.Protein;
            existing.SpecialComments = item.SpecialComments;
            existing.ImageUrl = item.ImageUrl;

            // נעדכן רק את TimeMealsString – זה שמישמר במסד
          

            // ⚠️ לא נעדכן Customers – אלא אם באמת יש צורך, וגם אז בזהירות עם Tracking
            // existing.Customers = item.Customers;

            _context.Save();
        }

    }
}