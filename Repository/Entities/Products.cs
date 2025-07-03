using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repository.Entities.CustomerFoodPreference;

namespace Repository.Entities
{

    public class Product
    {
        [Key]
        public int ProductId { get; set; } // מזהה מהמוצר או מה-API

        [Required]
        public string Name { get; set; }

        public double? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Carbohydrates { get; set; }

        public string? SourceApi { get; set; } // למשל "OpenFoodAPI"

        public virtual ICollection<CustomerFoodPreference> CustomerFoodPreferences { get; set; }
        public virtual ICollection<ProductForDietType> ProductForDietType { get; set; }
    }

}

