namespace GroupProj2_321.Models
{
    public class Produce
    {
        public int ProduceId { get; set; }
        public int FarmerId { get; set; }
        public string ProduceName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal PricePerUnit { get; set; }
        public int AvailableQuantity { get; set; }
        public string Unit { get; set; } = string.Empty; // e.g., "lb", "bushel", "case"
        public bool FarmerCanDeliver { get; set; }
        public DateTime AvailabilityStart { get; set; }
        public DateTime AvailabilityEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public Farmer? Farmer { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

