using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductInsertModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int MinLevel { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Tags { get; set; }

        public string Notes { get; set; }

        [Required(ErrorMessage = "Please upload an image.")]
        public IFormFile Image { get; set; }

        public decimal TotalValue { get; set; }
    }
}
