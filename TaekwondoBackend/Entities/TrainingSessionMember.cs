namespace TaekwondoBackend.Entities
{
    public class TrainingSessionMember
    {
        public int TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; } = null!;

        public int MemberId { get; set; }
        public Members Member { get; set; } = null!;
    }
}
