namespace TaekwondoBackend.Entities
{
    public class UserMember
    {
        public Guid UserId { get; set; }
        public required User User { get; set; }

        public int MemberId { get; set; }
        public required Members Member { get; set; }

        public string? Type { get; set; }
    }
}
