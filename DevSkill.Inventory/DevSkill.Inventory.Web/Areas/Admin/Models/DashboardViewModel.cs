using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int ItemCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public List<Product> Products { get; set; }
    }
}
