using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class CustomerDiet
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int DietId { get; set; }
        public DietType Diet { get; set; }

    }
}
