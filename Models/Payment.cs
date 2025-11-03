namespace GroupProj2_321.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; } = "Pending"; // Successful, Pending, Failed, Refunded
        
        // Navigation properties
        public Order? Order { get; set; }
    }
}

