using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FieldsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Fields
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Field>>> GetFields()
        {
            return await _context.Fields.Include(f => f.SoilType).ToListAsync();
        }

        // GET: api/Fields/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Field>> GetField(int id)
        {
            var field = await _context.Fields
                .Include(f => f.SoilType)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (field == null)
            {
                return NotFound();
            }

            return field;
        }

        // POST: api/Fields
        [HttpPost]
        public async Task<ActionResult<Field>> CreateField(Field field)
        {
            field.CreatedAt = DateTime.UtcNow;
            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetField), new { id = field.Id }, field);
        }

        // PUT: api/Fields/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateField(int id, Field field)
        {
            if (id != field.Id)
            {
                return BadRequest();
            }

            field.UpdatedAt = DateTime.UtcNow;
            _context.Entry(field).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldExists(id))
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

        // DELETE: api/Fields/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteField(int id)
        {
            var field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FieldExists(int id)
        {
            return _context.Fields.Any(e => e.Id == id);
        }
    }
}