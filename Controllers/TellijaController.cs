using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TellijadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TellijadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tellija>>> GetTellijad()
        {
            return await _context.Tellijad.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tellija>> GetTellija(int id)
        {
            var tellija = await _context.Tellijad.FindAsync(id);
            if (tellija == null) return NotFound();
            return tellija;
        }

        [HttpPost]
        public async Task<ActionResult<Tellija>> PostTellija(Tellija tellija)
        {
            tellija.KasutajaTüüp = KasutajaTüüp.Tellija;
            _context.Tellijad.Add(tellija);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTellija", new { id = tellija.Id }, tellija);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTellija(int id, Tellija tellija)
        {
            if (id != tellija.Id) return BadRequest();
            tellija.KasutajaTüüp = KasutajaTüüp.Tellija;

            _context.Entry(tellija).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTellija(int id)
        {
            var tellija = await _context.Tellijad.FindAsync(id);
            if (tellija == null) return NotFound();

            _context.Tellijad.Remove(tellija);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}