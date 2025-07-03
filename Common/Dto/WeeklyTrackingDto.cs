using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class WeeklyTrackingDto
    {
        public int Id { get; set; }
        public int CustId { get; set; }
        public DateTime WeekDate { get; set; }
        public double UpdatdedWieght { get; set; }
        public bool IsPassCalories { get; set; }
        public int? ConsumedCalories { get; set; } 

    }
}
