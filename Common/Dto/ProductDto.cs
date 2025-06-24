using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
        public class ProductDto
        {
            public int  ProductId { get; set; }
            public string ?Name { get; set; }
            public double? Calories { get; set; }
            public double? Protein { get; set; }
            public double? Fat { get; set; }
            public double? Carbohydrates { get; set; }
            public string? SourceApi { get; set; }
        }
    }


