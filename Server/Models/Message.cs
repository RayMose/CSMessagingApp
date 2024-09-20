

namespace CSMessagingApp.Server.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CustomerName { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageStatus Status { get; set; }
        public int? AssignedAgentId { get; set; }
        public Agent AssignedAgent { get; set; }
    }

    public enum MessageStatus
    {
        Pending,
        InProgress,
        Resolved
    }
}
