namespace CSMessagingApp.Server.Models
{
    public class Agent
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
    }
}
