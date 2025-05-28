namespace TaekwondoBackend.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsVisible { get; set; } = true;

        public string? ImageUrl { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
