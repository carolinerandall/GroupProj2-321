namespace GroupProj2_321.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        public string FarmName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? BankAccountLast4 { get; set; }
        public string? BankAccountToken { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public List<Produce> Produces { get; set; } = new List<Produce>();
        public List<Order> Orders { get; set; } = new List<Order>();
        
        // Helper properties for display
        public string FullName => $"{FirstName} {LastName}";
    }
}

