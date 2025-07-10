using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using System.Threading.Tasks;

namespace Mock
{
    public class Database : DbContext, IContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DietType> DietTypes { get; set; }
        public DbSet<ProductForDietType> ProductForDietTypes { get; set; }
        public DbSet<WeeklyTracking> WeeklyTrackings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerFoodPreference> CustomerFoodPreferences { get; set; }
        public DbSet<CustomerDiet> CustomerDiet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // קשר בין Customer ל־CustomerFoodPreference
            modelBuilder.Entity<CustomerFoodPreference>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.FoodPreferences)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // למחוק העדפות כשנמחק לקוח

            // קשר בין Product ל־CustomerFoodPreference
            modelBuilder.Entity<CustomerFoodPreference>()
                .HasOne(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<CustomerDiet>()
    .HasKey(cd => new { cd.CustomerId, cd.DietId });
            modelBuilder.Entity<CustomerDiet>()
                .HasOne(cd => cd.Customer)
                .WithMany()
                .HasForeignKey(cd => cd.CustomerId);

            modelBuilder.Entity<CustomerDiet>()
                .HasOne(cd => cd.Diet)
                .WithMany()
                .HasForeignKey(cd => cd.DietId);

        }

        public void Save()
        {
            SaveChanges();
        }

        public async Task SaveAsync()
        {
            await SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-1VUANBN;Database=DietSC2;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
