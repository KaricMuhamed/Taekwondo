namespace TaekwondoBackend.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Members Member { get; set; } = null!;

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public string? PaymentMethod { get; set; } 

        public string? Notes { get; set; }

        public int PaymentMonth { get; set; } 
        public int PaymentYear { get; set; }
    }
}
