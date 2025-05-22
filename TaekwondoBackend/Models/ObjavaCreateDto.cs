namespace TaekwondoBackend.Models
{
    public class ObjavaCreateDto
    {
        public string Naslov { get; set; }
        public string Sadrzaj { get; set; }
        public string Tip { get; set; }
        public string ImageUrl { get; set; }
        public Guid AutorId { get; set; }
    }
}
