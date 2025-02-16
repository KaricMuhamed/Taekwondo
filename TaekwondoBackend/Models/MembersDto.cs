namespace TaekwondoBackend.Models
{
    public class MembersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int ContactNumber { get; set; }
        public string Adresse { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int CurrentBeltId { get; set; }
    }
}
