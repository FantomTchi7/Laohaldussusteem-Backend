using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Helpers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TellijaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TellijaController(ApplicationDbContext context)
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
            if (await _context.Kasutajad.AnyAsync(u => u.Email == tellija.Email))
            {
                return BadRequest("Email on juba kasutusel.");
            }

            tellija.KasutajaTüüp = KasutajaTüüp.Tellija;
            tellija.Parool = ParoolHelper.HashPassword(tellija.Parool);

            _context.Tellijad.Add(tellija);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTellija", new { id = tellija.Id }, tellija);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTellija(int id, Tellija updatedTellija)
        {
            if (id != updatedTellija.Id) return BadRequest();

            var existingTellija = await _context.Tellijad.FindAsync(id);
            if (existingTellija == null) return NotFound();

            existingTellija.Nimi = updatedTellija.Nimi;
            existingTellija.Email = updatedTellija.Email;

            if (!string.IsNullOrEmpty(updatedTellija.Parool))
            {
                existingTellija.Parool = ParoolHelper.HashPassword(updatedTellija.Parool);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TellijaExists(id)) return NotFound();
                else throw;
            }

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

        private bool TellijaExists(int id)
        {
            return _context.Tellijad.Any(e => e.Id == id);
        }
    }
}