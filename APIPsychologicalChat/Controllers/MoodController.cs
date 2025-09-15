using APIPsychologicalChat.DataBase;
using APIPsychologicalChat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIPsychologicalChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoodController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MoodController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("entry")]
        public async Task<ActionResult<MoodEntry>> AddMoodEntry([FromBody] MoodEntry entry)
        {
            entry.Id = Guid.NewGuid();
            entry.CreatedAt = DateTime.UtcNow;

            _context.MoodEntries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(entry);
        }

        [HttpGet("history/{userId}")]
        public async Task<ActionResult<IEnumerable<MoodEntry>>> GetMoodHistory(Guid userId)
        {
            var entries = await _context.MoodEntries
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return Ok(entries);
        }

        [HttpGet("stats/{userId}")]
        public async Task<ActionResult<object>> GetMoodStats(Guid userId)
        {
            var lastWeekEntries = await _context.MoodEntries
                .Where(m => m.UserId == userId && m.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .OrderBy(m => m.CreatedAt)
                .ToListAsync();

            var averageMood = lastWeekEntries.Any() ?
                lastWeekEntries.Average(m => m.MoodLevel) : 0;

            return Ok(new
            {
                AverageMood = Math.Round(averageMood, 1),
                Entries = lastWeekEntries
            });
        }
    }
}
