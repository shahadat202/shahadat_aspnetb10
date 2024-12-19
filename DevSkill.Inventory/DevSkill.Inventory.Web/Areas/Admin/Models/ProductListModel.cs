using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductListModel : DataTables
    {
        public ProductSearchDto SearchItem { get; set; }
        public string? Order { get; set; }
    }
}
