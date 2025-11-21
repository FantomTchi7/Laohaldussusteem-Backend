using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Helpers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TootedController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TootedController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Toode>>> GetTooted()
        {
            return await _context.Tooted.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Toode>> GetToode(int id)
        {
            var toode = await _context.Tooted.FindAsync(id);
            if (toode == null) return NotFound();
            return toode;
        }

        [HttpPost]
        public async Task<ActionResult<Toode>> PostToode(Toode toode)
        {
            _context.Tooted.Add(toode);
            await _context.SaveChangesAsync();

            await RecalculateArveTotals(toode.ArveId);

            return CreatedAtAction("GetToode", new { id = toode.Id }, toode);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutToode(int id, Toode toode)
        {
            if (id != toode.Id) return BadRequest();

            _context.Entry(toode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await RecalculateArveTotals(toode.ArveId);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tooted.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToode(int id)
        {
            var toode = await _context.Tooted.FindAsync(id);
            if (toode == null) return NotFound();

            int parentArveId = toode.ArveId;

            _context.Tooted.Remove(toode);
            await _context.SaveChangesAsync();

            await RecalculateArveTotals(parentArveId);

            return NoContent();
        }

        private async Task RecalculateArveTotals(int arveId)
        {
            var arve = await _context.Arved
                .Include(a => a.Tooted)
                .FirstOrDefaultAsync(a => a.Id == arveId);

            if (arve != null)
            {
                ArveCalculator.CalculateTotals(arve);
                _context.Entry(arve).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}