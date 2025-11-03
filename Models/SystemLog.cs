namespace GroupProj2_321.Models
{
    public class SystemLog
    {
        public int LogId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? IpAddress { get; set; }
    }
}

