namespace TaekwondoBackend.Models
{
    public class GrupaCreateDto
    {
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public List<Guid> UcenikIds { get; set; } = new List<Guid>();
    }
}
