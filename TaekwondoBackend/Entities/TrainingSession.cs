namespace TaekwondoBackend.Entities
{
    public class TrainingSession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid InstructorId { get; set; }
        public User Instructor { get; set; } = null!;

        public int? GroupId { get; set; }
        public Group? Group { get; set; }

        public ICollection<TrainingSessionMember> TrainingSessionMembers { get; set; } = new List<TrainingSessionMember>();
    }

}
