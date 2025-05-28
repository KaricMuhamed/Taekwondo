namespace TaekwondoBackend.Entities
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }      
        public DateTime EndDate { get; set; }        
        public string Location { get; set; } = null!;
        public string Description { get; set; } = null!;

        public int? GroupId { get; set; }
        public Group? Group { get; set; }

        public List<TournamentMember> TournamentMembers { get; set; } = new();
    }


}
