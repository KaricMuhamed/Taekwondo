namespace TaekwondoBackend.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<GroupMember>? GroupMembers { get; set; }
    }
}
