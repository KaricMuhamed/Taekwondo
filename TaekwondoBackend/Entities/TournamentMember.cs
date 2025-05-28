namespace TaekwondoBackend.Entities
{
    public class TournamentMember
    {
        public int Id { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; } = null!;

        public int MemberId { get; set; }
        public Members Member { get; set; } = null!;
    }

}
