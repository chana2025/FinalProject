using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Repository.Entities
{
    public class DietType
    {
        [Key]
        public int Id { get; set; }
        public string DietName { get; set; }
        public double? Calories { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        public double? Carbohydrates { get; set; }
        public string? SpecialComments { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
//public virtual ICollection<ProductForDietType> ProductForDietTypes { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }     
        public string? SampleMenu { get; set; }

    }
}