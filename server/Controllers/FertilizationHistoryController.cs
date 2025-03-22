using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FertilizationHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FertilizationHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FertilizationHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FertilizationHistory>>> GetFertilizationHistory()
        {
            return await _context.FertilizationHistory
                .Include(fh => fh.Field)
                .Include(fh => fh.Fertilizer)
                .Include(fh => fh.User)
                .ToListAsync();
        }

        // GET: api/FertilizationHistory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FertilizationHistory>> GetFertilizationHistory(int id)
        {
            var history = await _context.FertilizationHistory
                .Include(fh => fh.Field)
                .Include(fh => fh.Fertilizer)
                .Include(fh => fh.User)
                .FirstOrDefaultAsync(fh => fh.Id == id);

            if (history == null)
            {
                return NotFound();
            }

            return history;
        }

        // POST: api/FertilizationHistory
        [HttpPost]
        public async Task<ActionResult<FertilizationHistory>> CreateFertilizationHistory(FertilizationHistory history)
        {
            history.CreatedAt = DateTime.UtcNow;
            _context.FertilizationHistory.Add(history);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFertilizationHistory), new { id = history.Id }, history);
        }

        // PUT: api/FertilizationHistory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFertilizationHistory(int id, FertilizationHistory history)
        {
            if (id != history.Id)
            {
                return BadRequest();
            }

            history.UpdatedAt = DateTime.UtcNow;
            _context.Entry(history).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FertilizationHistoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/FertilizationHistory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFertilizationHistory(int id)
        {
            var history = await _context.FertilizationHistory.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            _context.FertilizationHistory.Remove(history);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FertilizationHistoryExists(int id)
        {
            return _context.FertilizationHistory.Any(e => e.Id == id);
        }
    }
}