namespace TaekwondoBackend.Models
{
    public class TreningResponseDto
    {
        public Guid Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan VrijemeOd { get; set; }
        public TimeSpan VrijemeDo { get; set; }
        public string Lokacija { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<KorisnikResponseDto> Treneri { get; set; } = new List<KorisnikResponseDto>();
        public List<GrupaResponseDto> Grupe { get; set; } = new List<GrupaResponseDto>();
    }
}
