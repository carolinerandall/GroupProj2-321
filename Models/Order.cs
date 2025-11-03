namespace GroupProj2_321.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int SchoolId { get; set; }
        public int FarmerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Shipped, Delivered, Cancelled
        public decimal TotalCost { get; set; }
        public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid, Paid, Refunded, Partial
        public DateTime CreatedAt { get; set; }
        public decimal DeliveryFee { get; set; } = 0;
        
        // Display properties for email contacts (not stored in DB, populated from joins)
        public string? FarmerEmail { get; set; }
        public string? FarmerName { get; set; }
        public string? SchoolEmail { get; set; }
        public string? SchoolName { get; set; }
        public string? SchoolContactName { get; set; }
        
        // Navigation properties
        public School? School { get; set; }
        public Farmer? Farmer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Delivery? Delivery { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}

