namespace TaekwondoBackend.Models
{
    public class ObjavaResponseDto
    { 
        public Guid Id { get; set; }
        public string Naslov { get; set; }
        public string Sadrzaj { get; set; }
        public string Tip { get; set; }
        public string ImageUrl { get; set; }
        public Guid AutorId { get; set; }
        public string AutorIme { get; set; } // Include author name without full navigation
        public DateTime Objavljeno { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
