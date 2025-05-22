namespace TaekwondoBackend.Models
{
    public class KorisnikResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Ime { get; set; }
        public string Telefon { get; set; }
        public string Uloga { get; set; }
        public string Stanje { get; set; }
        public string Pojas { get; set; }
        public DateTime DatumPridruzivanja { get; set; }
        public string Napomene { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
