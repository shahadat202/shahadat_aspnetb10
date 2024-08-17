namespace Blog.Domain.Entities
{
    public class BlogPost : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}
