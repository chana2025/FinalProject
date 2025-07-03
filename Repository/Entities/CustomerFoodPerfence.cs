using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Entities;

namespace Repository.Entities
{

    //public class CustomerFoodPreference
    //{
    //    public int Id { get; set; }
    //    public int CustomerId { get; set; }
    //    public int ProductId { get; set; }

    //    // השדה שמבדיל בין אהוב לשנוא
    //    public bool IsLiked { get; set; }

    //    // קשרים לאובייקטים אחרים במידה ויש
    //    public Product Product { get; set; }
    //    public Customer Customer { get; set; }
    //}
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public class CustomerFoodPreference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [Required]
        public bool IsLiked { get; set; }
    }
}


