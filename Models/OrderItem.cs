namespace GroupProj2_321.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProduceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        
        // Navigation properties
        public Order? Order { get; set; }
        public Produce? Produce { get; set; }
    }
}

