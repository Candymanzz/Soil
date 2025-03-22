using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CropsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CropsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Crops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Crop>>> GetCrops()
        {
            return await _context.Crops.ToListAsync();
        }

        // GET: api/Crops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Crop>> GetCrop(int id)
        {
            var crop = await _context.Crops.FindAsync(id);

            if (crop == null)
            {
                return NotFound();
            }

            return crop;
        }

        // POST: api/Crops
        [HttpPost]
        public async Task<ActionResult<Crop>> CreateCrop(Crop crop)
        {
            crop.CreatedAt = DateTime.UtcNow;
            _context.Crops.Add(crop);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCrop), new { id = crop.Id }, crop);
        }

        // PUT: api/Crops/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCrop(int id, Crop crop)
        {
            if (id != crop.Id)
            {
                return BadRequest();
            }

            crop.UpdatedAt = DateTime.UtcNow;
            _context.Entry(crop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CropExists(id))
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

        // DELETE: api/Crops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrop(int id)
        {
            var crop = await _context.Crops.FindAsync(id);
            if (crop == null)
            {
                return NotFound();
            }

            _context.Crops.Remove(crop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CropExists(int id)
        {
            return _context.Crops.Any(e => e.Id == id);
        }
    }
}