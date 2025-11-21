using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Helpers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArvedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArvedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Arve>>> GetArved()
        {
            return await _context.Arved
                .Include(a => a.Koostaja)
                .Include(a => a.Tellija)
                .Include(a => a.Ettevõte)
                .Include(a => a.Tooted)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Arve>> GetArve(int id)
        {
            var arve = await _context.Arved
                .Include(a => a.Koostaja)
                .Include(a => a.Tellija)
                .Include(a => a.Ettevõte)
                .Include(a => a.Tooted)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (arve == null) return NotFound();

            return arve;
        }

        [HttpPost]
        public async Task<ActionResult<Arve>> PostArve(Arve arve)
        {
            ArveCalculator.CalculateTotals(arve);

            _context.Arved.Add(arve);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArve", new { id = arve.Id }, arve);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArve(int id, Arve arve)
        {
            if (id != arve.Id) return BadRequest();

            _context.Entry(arve).State = EntityState.Modified;

            try
            {
                if (arve.Tooted == null || !arve.Tooted.Any())
                {
                    var existingProducts = await _context.Tooted
                        .Where(t => t.ArveId == id)
                        .AsNoTracking()
                        .ToListAsync();

                    arve.Tooted = existingProducts;
                }

                ArveCalculator.CalculateTotals(arve);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArveExists(id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArve(int id)
        {
            var arve = await _context.Arved.FindAsync(id);
            if (arve == null) return NotFound();

            _context.Arved.Remove(arve);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArveExists(int id)
        {
            return _context.Arved.Any(e => e.Id == id);
        }
    }
}