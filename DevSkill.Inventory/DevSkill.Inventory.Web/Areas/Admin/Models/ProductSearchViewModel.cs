using DevSkill.Inventory.Domain.Entities;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductSearchViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public ProductListModel ProductListModel { get; set; }
    }

}
