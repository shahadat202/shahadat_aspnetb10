namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductSearchModel
    {
        public string Title { get; set; }
        public int? Quantity { get; set; }
        public int? MinLevel { get; set; }
        public string Tag { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }

}
