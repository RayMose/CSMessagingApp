using CSMessagingApp.Server.Data;
using CSMessagingApp.Server.Models;

namespace CSMessagingApp.Server.Services
{
    public class MessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetPendingMessagesAsync()
        {
            return await _context.Messages
                .Where(m => m.Status == MessageStatus.Pending)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            message.Timestamp = DateTime.UtcNow;
            message.Status = MessageStatus.Pending;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> AssignMessageAsync(int messageId, int agentId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null)
                throw new ArgumentException("Message not found");

            message.AssignedAgentId = agentId;
            message.Status = MessageStatus.InProgress;
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> ResolveMessageAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null)
                throw new ArgumentException("Message not found");

            message.Status = MessageStatus.Resolved;
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
