using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoilTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SoilTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SoilTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SoilType>>> GetSoilTypes()
        {
            return await _context.SoilTypes.ToListAsync();
        }

        // GET: api/SoilTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SoilType>> GetSoilType(int id)
        {
            var soilType = await _context.SoilTypes.FindAsync(id);

            if (soilType == null)
            {
                return NotFound();
            }

            return soilType;
        }

        // POST: api/SoilTypes
        [HttpPost]
        public async Task<ActionResult<SoilType>> CreateSoilType(SoilType soilType)
        {
            soilType.CreatedAt = DateTime.UtcNow;
            _context.SoilTypes.Add(soilType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSoilType), new { id = soilType.Id }, soilType);
        }

        // PUT: api/SoilTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSoilType(int id, SoilType soilType)
        {
            if (id != soilType.Id)
            {
                return BadRequest();
            }

            soilType.UpdatedAt = DateTime.UtcNow;
            _context.Entry(soilType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SoilTypeExists(id))
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

        // DELETE: api/SoilTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoilType(int id)
        {
            var soilType = await _context.SoilTypes.FindAsync(id);
            if (soilType == null)
            {
                return NotFound();
            }

            _context.SoilTypes.Remove(soilType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SoilTypeExists(int id)
        {
            return _context.SoilTypes.Any(e => e.Id == id);
        }
    }
}