using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FertilizationPlansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FertilizationPlansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FertilizationPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FertilizationPlan>>> GetFertilizationPlans()
        {
            return await _context.FertilizationPlans
                .Include(fp => fp.Field)
                .Include(fp => fp.Crop)
                .Include(fp => fp.Fertilizer)
                .ToListAsync();
        }

        // GET: api/FertilizationPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FertilizationPlan>> GetFertilizationPlan(int id)
        {
            var plan = await _context.FertilizationPlans
                .Include(fp => fp.Field)
                .Include(fp => fp.Crop)
                .Include(fp => fp.Fertilizer)
                .FirstOrDefaultAsync(fp => fp.Id == id);

            if (plan == null)
            {
                return NotFound();
            }

            return plan;
        }

        // POST: api/FertilizationPlans
        [HttpPost]
        public async Task<ActionResult<FertilizationPlan>> CreateFertilizationPlan(FertilizationPlan plan)
        {
            plan.CreatedAt = DateTime.UtcNow;
            _context.FertilizationPlans.Add(plan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFertilizationPlan), new { id = plan.Id }, plan);
        }

        // PUT: api/FertilizationPlans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFertilizationPlan(int id, FertilizationPlan plan)
        {
            if (id != plan.Id)
            {
                return BadRequest();
            }

            plan.UpdatedAt = DateTime.UtcNow;
            _context.Entry(plan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FertilizationPlanExists(id))
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

        // DELETE: api/FertilizationPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFertilizationPlan(int id)
        {
            var plan = await _context.FertilizationPlans.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }

            _context.FertilizationPlans.Remove(plan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FertilizationPlanExists(int id)
        {
            return _context.FertilizationPlans.Any(e => e.Id == id);
        }
    }
}