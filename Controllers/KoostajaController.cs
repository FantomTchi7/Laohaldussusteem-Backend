using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoostajadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KoostajadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Koostaja>>> GetKoostajad()
        {
            return await _context.Koostajad.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Koostaja>> GetKoostaja(int id)
        {
            var koostaja = await _context.Koostajad.FindAsync(id);
            if (koostaja == null) return NotFound();
            return koostaja;
        }

        [HttpPost]
        public async Task<ActionResult<Koostaja>> PostKoostaja(Koostaja koostaja)
        {
            koostaja.KasutajaTüüp = KasutajaTüüp.Koostaja;
            _context.Koostajad.Add(koostaja);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetKoostaja", new { id = koostaja.Id }, koostaja);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKoostaja(int id, Koostaja koostaja)
        {
            if (id != koostaja.Id) return BadRequest();
            koostaja.KasutajaTüüp = KasutajaTüüp.Koostaja;

            _context.Entry(koostaja).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKoostaja(int id)
        {
            var koostaja = await _context.Koostajad.FindAsync(id);
            if (koostaja == null) return NotFound();

            _context.Koostajad.Remove(koostaja);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}