using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Helpers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArveController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArveController(ApplicationDbContext context)
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
            ArveHelper.CalculateTotals(arve);

            _context.Arved.Add(arve);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArve", new { id = arve.Id }, arve);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArve(int id, Arve incomingArve)
        {
            if (id != incomingArve.Id) return BadRequest();

            var existingArve = await _context.Arved
                .Include(a => a.Tooted)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingArve == null) return NotFound();

            _context.Entry(existingArve).CurrentValues.SetValues(incomingArve);

            var incomingProductIds = incomingArve.Tooted.Select(t => t.Id).Where(i => i != 0).ToList();
            
            var productsToDelete = existingArve.Tooted
                .Where(t => !incomingProductIds.Contains(t.Id))
                .ToList();

            foreach (var prod in productsToDelete)
            {
                _context.Tooted.Remove(prod);
            }

            foreach (var incomingItem in incomingArve.Tooted)
            {
                if (incomingItem.Id == 0)
                {
                    incomingItem.ArveId = existingArve.Id;
                    existingArve.Tooted.Add(incomingItem);
                }
                else
                {
                    var existingItem = existingArve.Tooted.FirstOrDefault(t => t.Id == incomingItem.Id);
                    if (existingItem != null)
                    {
                        existingItem.Nimetus = incomingItem.Nimetus;
                        existingItem.Ühik = incomingItem.Ühik;
                        existingItem.Kogus = incomingItem.Kogus;
                        existingItem.Hind = incomingItem.Hind;
                        existingItem.LaduId = incomingItem.LaduId;
                        existingItem.BaasHind = incomingItem.BaasHind; 
                    }
                }
            }

            ArveHelper.CalculateTotals(existingArve);

            try
            {
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