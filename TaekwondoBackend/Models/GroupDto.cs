namespace TaekwondoBackend.Models
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<int> MemberIds { get; set; } = new();
    }

    public class CreateGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public List<int>? MemberIds { get; set; } = new();
    }

    public class UpdateGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public List<int>? MemberIds { get; set; } = new();
    }
}
