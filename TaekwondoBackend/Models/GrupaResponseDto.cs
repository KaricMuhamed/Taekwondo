namespace TaekwondoBackend.Models
{
    public class GrupaResponseDto
    {
        public Guid Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<KorisnikResponseDto> Ucenici { get; set; } = new List<KorisnikResponseDto>();
    }
}
