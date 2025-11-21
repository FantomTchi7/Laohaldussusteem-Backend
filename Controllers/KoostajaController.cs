using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Helpers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KoostajaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KoostajaController(ApplicationDbContext context)
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
            if (await _context.Kasutajad.AnyAsync(u => u.Email == koostaja.Email))
            {
                return BadRequest("Email on juba kasutusel.");
            }

            koostaja.KasutajaTüüp = KasutajaTüüp.Koostaja;
            koostaja.Parool = ParoolHelper.HashPassword(koostaja.Parool);

            _context.Koostajad.Add(koostaja);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetKoostaja", new { id = koostaja.Id }, koostaja);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKoostaja(int id, Koostaja updatedKoostaja)
        {
            if (id != updatedKoostaja.Id) return BadRequest();

            var existingKoostaja = await _context.Koostajad.FindAsync(id);
            if (existingKoostaja == null) return NotFound();

            existingKoostaja.Nimi = updatedKoostaja.Nimi;
            existingKoostaja.Email = updatedKoostaja.Email;
            existingKoostaja.Skype = updatedKoostaja.Skype;
            existingKoostaja.Telefon = updatedKoostaja.Telefon;

            if (!string.IsNullOrEmpty(updatedKoostaja.Parool))
            {
                existingKoostaja.Parool = ParoolHelper.HashPassword(updatedKoostaja.Parool);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KoostajaExists(id)) return NotFound();
                else throw;
            }

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

        private bool KoostajaExists(int id)
        {
            return _context.Koostajad.Any(e => e.Id == id);
        }
    }
}