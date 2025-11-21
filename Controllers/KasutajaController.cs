using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KasutajaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KasutajaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kasutaja>>> GetKasutajad()
        {
            return await _context.Kasutajad.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kasutaja>> GetKasutaja(int id)
        {
            var kasutaja = await _context.Kasutajad.FindAsync(id);
            if (kasutaja == null) return NotFound();
            return kasutaja;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKasutaja(int id)
        {
            var kasutaja = await _context.Kasutajad.FindAsync(id);
            if (kasutaja == null) return NotFound();
            _context.Kasutajad.Remove(kasutaja);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}