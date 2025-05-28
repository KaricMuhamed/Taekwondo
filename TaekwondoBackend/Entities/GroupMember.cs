namespace TaekwondoBackend.Entities
{
    public class GroupMember
    {
        public int GroupId { get; set; }
        public Group? Group { get; set; }
        public int MemberId { get; set; }
        public Members? Member { get; set; }
    }
}
