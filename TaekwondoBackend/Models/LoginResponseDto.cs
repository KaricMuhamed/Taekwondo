namespace TaekwondoBackend.Models
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public KorisnikResponseDto? Korisnik { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiration { get; set; }

    }
}