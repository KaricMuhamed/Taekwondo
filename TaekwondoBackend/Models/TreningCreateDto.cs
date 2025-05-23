namespace TaekwondoBackend.Models
{
    public class TreningCreateDto
    {
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan VrijemeOd { get; set; }
        public TimeSpan VrijemeDo { get; set; }
        public string Lokacija { get; set; }
        public List<Guid> TrenerIds { get; set; } = new List<Guid>();
        public List<Guid> GrupaIds { get; set; } = new List<Guid>();
    }
}
