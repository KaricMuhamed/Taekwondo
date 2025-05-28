namespace TaekwondoBackend.Models
{
    public class TournamentDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }     
        public DateTime EndDate { get; set; }        
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int? GroupId { get; set; }
        public List<MemberDto>? Members { get; set; }
    }


}
