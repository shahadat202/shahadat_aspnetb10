using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }         
        public string Title { get; set; }      
        public int Quantity { get; set; }      
        public int MinLevel { get; set; }      
        public decimal Price { get; set; }   
        public string Tags { get; set; }      
        public string Notes { get; set; }     
        public string? Image { get; set; }  
        public DateTime CreatedDate { get; set; } 
        public decimal TotalValue { get; set; } 
    }
}
