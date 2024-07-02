namespace Inventory.Domain.Entities
{
    public class BlogPost
    {
        public string Title { get; set; }
        public List<Comment> Comments { get; set; }
        public BlogPost()
        {

        }
    }
}
