using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<DietType> DietTypes { get; set; }
        DbSet<WeeklyTracking> WeeklyTrackings { get; set; }
        DbSet<ProductForDietType> ProductForDietTypes { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<CustomerFoodPreference> CustomerFoodPreferences { get; set; }
        DbSet<CustomerDiet> CustomerDiet { get; set; }

        void Save();
        Task SaveAsync(); // ⬅️ הוספה זו מאפשרת שימוש ב-Await במאגר
    }
}
