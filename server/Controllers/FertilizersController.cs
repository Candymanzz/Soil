using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FertilizersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FertilizersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Fertilizers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fertilizer>>> GetFertilizers()
        {
            return await _context.Fertilizers.ToListAsync();
        }

        // GET: api/Fertilizers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fertilizer>> GetFertilizer(int id)
        {
            var fertilizer = await _context.Fertilizers.FindAsync(id);

            if (fertilizer == null)
            {
                return NotFound();
            }

            return fertilizer;
        }

        // POST: api/Fertilizers
        [HttpPost]
        public async Task<ActionResult<Fertilizer>> CreateFertilizer(Fertilizer fertilizer)
        {
            fertilizer.CreatedAt = DateTime.UtcNow;
            _context.Fertilizers.Add(fertilizer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFertilizer), new { id = fertilizer.Id }, fertilizer);
        }

        // PUT: api/Fertilizers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFertilizer(int id, Fertilizer fertilizer)
        {
            if (id != fertilizer.Id)
            {
                return BadRequest();
            }

            fertilizer.UpdatedAt = DateTime.UtcNow;
            _context.Entry(fertilizer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FertilizerExists(id))
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

        // DELETE: api/Fertilizers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFertilizer(int id)
        {
            var fertilizer = await _context.Fertilizers.FindAsync(id);
            if (fertilizer == null)
            {
                return NotFound();
            }

            _context.Fertilizers.Remove(fertilizer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FertilizerExists(int id)
        {
            return _context.Fertilizers.Any(e => e.Id == id);
        }
    }
}