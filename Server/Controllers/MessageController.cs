using CSMessagingApp.Server.Models;
using CSMessagingApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSMessagingApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _messageService;

        public MessageController(MessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("pending")]
        public async Task<ActionResult<List<Message>>> GetPendingMessages()
        {
            var messages = await _messageService.GetPendingMessagesAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult<Message>> AddMessage(Message message)
        {
            var addedMessage = await _messageService.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetPendingMessages), new { id = addedMessage.Id }, addedMessage);
        }

        [HttpPut("{id}/assign")]
        public async Task<ActionResult<Message>> AssignMessage(int id, [FromBody] int agentId)
        {
            try
            {
                var updatedMessage = await _messageService.AssignMessageAsync(id, agentId);
                return Ok(updatedMessage);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/resolve")]
        public async Task<ActionResult<Message>> ResolveMessage(int id)
        {
            try
            {
                var resolvedMessage = await _messageService.ResolveMessageAsync(id);
                return Ok(resolvedMessage);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}
