namespace TaekwondoBackend.Models
{
    public class KorisnikCreateDto
    {
        public string Email { get; set; }
        public string Password { get; set; }  // Plain password, not hashed
        public string Ime { get; set; }
        public string Telefon { get; set; }
        public string Uloga { get; set; }
        public string Stanje { get; set; } = "na_cekanju";
        public string Pojas { get; set; } = "bijeli";
        public string Napomene { get; set; }
    }
}
