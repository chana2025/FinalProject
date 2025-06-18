using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Repository.Entities.CustomerFoodPreference;

namespace Repository.Entities
{
    //database בפועל
    public enum eGender
    {
        MALE,
        FEMALE
    }
    //i added it's
    public enum eRole
    {
        ADMIN, WORKER, USER
    }

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        public eGender Gender { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public double? Height { get; set; }
        public double? Weight { get; set; }

        [Required]
        public string Password { get; set; }

        public int? DietId { get; set; }

        [ForeignKey("DietId")]
        public virtual DietType? DietType { get; set; }

        public virtual ICollection<WeeklyTracking> WeeklyTrackings { get; set; }

        public eRole Role { get; set; }
        public string? ImageUrl { get; set; }
        //GPT said to change to llist...
        //public virtual CustomerFoodPreference FoodPreferences { get; set; }
        public virtual ICollection<CustomerFoodPreference> FoodPreferences { get; set; }
        public byte[]? ImagePath { get; set; }


    }
}
