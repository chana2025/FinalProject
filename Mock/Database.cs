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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // קשר בין Customer ל־CustomerFoodPreference
            modelBuilder.Entity<CustomerFoodPreference>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.FoodPreferences)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // אופציונלי - למחוק העדפות כשנמחק לקוח

            // קשר בין Product ל־CustomerFoodPreference
            modelBuilder.Entity<CustomerFoodPreference>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);
        }

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
