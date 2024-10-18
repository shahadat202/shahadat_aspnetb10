using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public class ProductSearchDto
    {
        public string? Title { get; set; } 
        public int? Quantity { get; set; } 
        public int? MinLevel { get; set; } 
        public string? Tag { get; set; }
        public decimal? PriceFrom { get; set; } 
        public decimal? PriceTo { get; set; }
        public DateTime? DateFrom { get; set; } 
        public DateTime? DateTo { get; set; } 
    }

}
