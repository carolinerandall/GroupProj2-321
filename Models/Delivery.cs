namespace GroupProj2_321.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; }
        public string? TruckCompany { get; set; }
        public string? TruckContact { get; set; }
        public decimal DeliveryFeeTotal { get; set; }
        public decimal SchoolShare { get; set; } // 50% of delivery fee
        public decimal FarmerShare { get; set; } // 50% of delivery fee
        public string DeliveryStatus { get; set; } = "Scheduled"; // Scheduled, In Transit, Delivered, Cancelled
        public DateTime EstimatedArrival { get; set; }
        
        // Navigation properties
        public Order? Order { get; set; }
    }
}

