using APIPsychologicalChat.DataBase;
using APIPsychologicalChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIPsychologicalChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Создать нового анонимного пользователя
        [HttpPost("create")]
        public async Task<ActionResult<User>> CreateUser([FromBody] string? nickname = null)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                TemporaryNickname = nickname
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // Получить информацию о пользователе
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
