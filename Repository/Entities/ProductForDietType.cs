using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class ProductForDietType
    {
        [Key]
        public int Id { get; set; }

        public int DietTypeId { get; set; }
        [ForeignKey(nameof(DietTypeId))]
        public DietType DietType { get; set; }

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public bool IsAllowed { get; set; }
    }
}
