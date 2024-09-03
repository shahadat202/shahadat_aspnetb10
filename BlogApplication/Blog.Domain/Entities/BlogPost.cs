namespace Blog.Domain.Entities
{
    public class BlogPost : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PostDate { get; set; }
        public Category Category { get; set; }
    }
}
