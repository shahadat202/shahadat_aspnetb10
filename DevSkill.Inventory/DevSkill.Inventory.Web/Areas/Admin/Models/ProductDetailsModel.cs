namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductDetailsModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set; }
        public int MinLevel { get; set; }
        public string Tags { get; set; }
        public string Notes { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
