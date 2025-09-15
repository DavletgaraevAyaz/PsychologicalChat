using APIPsychologicalChat.DataBase;
using APIPsychologicalChat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIPsychologicalChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получить историю сообщений
        [HttpGet("history/{userId}")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetChatHistory(Guid userId)
        {
            var messages = await _context.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return Ok(messages);
        }

        // Отправить новое сообщение
        [HttpPost("send")]
        public async Task<ActionResult<ChatMessage>> SendMessage([FromBody] ChatMessage message)
        {
            message.Id = Guid.NewGuid();
            message.Timestamp = DateTime.UtcNow;

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChatHistory),
                new { userId = message.UserId }, message);
        }

        // Получить последние сообщения (для поддержки)
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetRecentMessages()
        {
            var messages = await _context.ChatMessages
                .OrderByDescending(m => m.Timestamp)
                .Take(50)
                .ToListAsync();

            return Ok(messages);
        }
    }
}
