using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Mock
{
    public class Database:DbContext ,IContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DietType> DietTypes { get; set; }
        public DbSet<ProductForDietType> ProductForDietTypes { get; set; }
        public DbSet<WeeklyTracking> WeeklyTrackings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerFoodPreference> CustomerFoodPreferences { get; set; }


        public void Save()
        {
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=DESKTOP-1VUANBN;Database=DietSC2;Trusted_Connection=True;TrustServerCertificate=True;");
        }

       
    }
}
