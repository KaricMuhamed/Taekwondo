public class TrainingSessionDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string InstructorId { get; set; } = string.Empty;
    public InstructorDto? Instructor { get; set; }
    public int? GroupId { get; set; }
    public List<MemberDto> Members { get; set; } = new();
}

public class InstructorDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class MemberDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}