using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherConditionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WeatherConditionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/WeatherConditions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherCondition>>> GetWeatherConditions()
        {
            return await _context.WeatherConditions
                .Include(wc => wc.Field)
                .ToListAsync();
        }

        // GET: api/WeatherConditions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherCondition>> GetWeatherCondition(int id)
        {
            var weather = await _context.WeatherConditions
                .Include(wc => wc.Field)
                .FirstOrDefaultAsync(wc => wc.Id == id);

            if (weather == null)
            {
                return NotFound();
            }

            return weather;
        }

        // POST: api/WeatherConditions
        [HttpPost]
        public async Task<ActionResult<WeatherCondition>> CreateWeatherCondition(WeatherCondition weather)
        {
            weather.CreatedAt = DateTime.UtcNow;
            _context.WeatherConditions.Add(weather);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWeatherCondition), new { id = weather.Id }, weather);
        }

        // PUT: api/WeatherConditions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeatherCondition(int id, WeatherCondition weather)
        {
            if (id != weather.Id)
            {
                return BadRequest();
            }

            weather.UpdatedAt = DateTime.UtcNow;
            _context.Entry(weather).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherConditionExists(id))
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

        // DELETE: api/WeatherConditions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherCondition(int id)
        {
            var weather = await _context.WeatherConditions.FindAsync(id);
            if (weather == null)
            {
                return NotFound();
            }

            _context.WeatherConditions.Remove(weather);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeatherConditionExists(int id)
        {
            return _context.WeatherConditions.Any(e => e.Id == id);
        }
    }
}