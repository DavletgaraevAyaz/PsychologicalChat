using APIPsychologicalChat.DataBase;
using APIPsychologicalChat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIPsychologicalChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SpecialistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Получить всех специалистов
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HelpSpecialist>>> GetSpecialists()
        {
            var specialists = await _context.HelpSpecialists
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return Ok(specialists);
        }

        // Получить специалиста по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<HelpSpecialist>> GetSpecialist(Guid id)
        {
            var specialist = await _context.HelpSpecialists
                .FirstOrDefaultAsync(s => s.Id == id);

            if (specialist == null)
                return NotFound();

            return Ok(specialist);
        }

        // Поиск специалистов по специализации
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<HelpSpecialist>>> SearchSpecialists(
            [FromQuery] string specialization)
        {
            var specialists = await _context.HelpSpecialists
                .Where(s => s.Specialization.Contains(specialization, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();

            return Ok(specialists);
        }

        // Добавить нового специалиста (для админов)
        [HttpPost]
        public async Task<ActionResult<HelpSpecialist>> AddSpecialist([FromBody] HelpSpecialist specialist)
        {
            specialist.Id = Guid.NewGuid();
            _context.HelpSpecialists.Add(specialist);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSpecialist),
                new { id = specialist.Id }, specialist);
        }
    }
}
