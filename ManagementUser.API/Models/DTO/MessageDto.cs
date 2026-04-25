namespace ManagementUser.API.Models.DTO
{
    public class MessageDto
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}