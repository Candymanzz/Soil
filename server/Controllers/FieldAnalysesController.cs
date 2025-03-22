using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FieldAnalysesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FieldAnalysesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FieldAnalyses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FieldAnalysis>>> GetFieldAnalyses()
        {
            return await _context.FieldAnalyses
                .Include(fa => fa.Field)
                .ToListAsync();
        }

        // GET: api/FieldAnalyses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FieldAnalysis>> GetFieldAnalysis(int id)
        {
            var analysis = await _context.FieldAnalyses
                .Include(fa => fa.Field)
                .FirstOrDefaultAsync(fa => fa.Id == id);

            if (analysis == null)
            {
                return NotFound();
            }

            return analysis;
        }

        // POST: api/FieldAnalyses
        [HttpPost]
        public async Task<ActionResult<FieldAnalysis>> CreateFieldAnalysis(FieldAnalysis analysis)
        {
            analysis.CreatedAt = DateTime.UtcNow;
            _context.FieldAnalyses.Add(analysis);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFieldAnalysis), new { id = analysis.Id }, analysis);
        }

        // PUT: api/FieldAnalyses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFieldAnalysis(int id, FieldAnalysis analysis)
        {
            if (id != analysis.Id)
            {
                return BadRequest();
            }

            analysis.UpdatedAt = DateTime.UtcNow;
            _context.Entry(analysis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FieldAnalysisExists(id))
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

        // DELETE: api/FieldAnalyses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFieldAnalysis(int id)
        {
            var analysis = await _context.FieldAnalyses.FindAsync(id);
            if (analysis == null)
            {
                return NotFound();
            }

            _context.FieldAnalyses.Remove(analysis);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FieldAnalysisExists(int id)
        {
            return _context.FieldAnalyses.Any(e => e.Id == id);
        }
    }
}