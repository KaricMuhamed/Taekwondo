namespace TaekwondoBackend.Models
{
    public class PaymentDto
    {
        public int MemberId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
    }
}
