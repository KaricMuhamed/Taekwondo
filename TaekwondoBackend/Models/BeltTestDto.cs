public class BeltTestDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = null!;

    public int CurrentBeltId { get; set; }
    public string CurrentBeltName { get; set; } = null!;

    public int AppliedBeltId { get; set; }
    public string AppliedBeltName { get; set; } = null!;

    public DateTime ScheduledDate { get; set; }
    public DateTime? TestedDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
}

public class BeltTestCreateDto
{
    public int MemberId { get; set; }
    public int AppliedBeltId { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string? Notes { get; set; }
}

public class BeltTestUpdateDto
{
    public DateTime? TestedDate { get; set; }
    public string Status { get; set; } = null!; // "Passed", "Failed", "Canceled", "Scheduled"
    public string? Notes { get; set; }
}
