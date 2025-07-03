using Microsoft.Extensions.DependencyInjection;
using Repository.Entities;
using Repository.Interfaces;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public static class ExtentionRepository
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            //תלויות
            services.AddScoped<IRepository<Customer>, CustomerRepository>();
            //לשחרר מהערה אחרי שמוסיפים את ה repositories

            services.AddScoped<IRepository<DietType>, DietTypeRepository>();
            //services.AddScoped<IRepository<ProductForDietType>, ProductForDietTypeRepository>();
            // services.AddScoped<IRepository<WeeklyTracking>, WeeklyTrackingRepository>();

            return services;
        }
    }
}
