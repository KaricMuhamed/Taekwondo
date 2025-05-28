namespace TaekwondoBackend.Models
{
    public class PostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsVisible { get; set; } = true;
        public string? ImageUrl { get; set; }
        public Guid UserId { get; set; }
    }
}
