namespace GroupProj2_321.Models
{
    public class School
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? CreditCardLast4 { get; set; }
        public string? CreditCardToken { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}

