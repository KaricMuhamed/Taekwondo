using TaekwondoBackend.Models;

namespace TaekwondoBackend.Entities
{
    public enum BeltTestStatus
    {
        Scheduled,
        Passed,
        Failed,
        Canceled
    }

    public class BeltTest
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Members Member { get; set; } = null!;

        public int CurrentBeltId { get; set; }  
        public Belts CurrentBelt { get; set; } = null!;

        public int AppliedBeltId { get; set; }  
        public Belts AppliedBelt { get; set; } = null!;

        public DateTime ScheduledDate { get; set; }  

        public DateTime? TestedDate { get; set; }  

        public BeltTestStatus Status { get; set; }  // Scheduled, Passed, Failed, Canceled

        public string? Notes { get; set; }
    }
}
