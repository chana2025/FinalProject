using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Entities
{
    public class WeeklyTracking
    {
        [Key]
        public int Id { get; set; }
        public int CustId { get; set; }
        [ForeignKey("CustId")]
        public Customer Customer { get; set; }
        public DateTime WeekDate { get; set; }
        public double UpdatdedWieght { get; set; }
        public bool IsPassCalories { get; set; }
        public int? ConsumedCalories { get; set; }

    }
}
