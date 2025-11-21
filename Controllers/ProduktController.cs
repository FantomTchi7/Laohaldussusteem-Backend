using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduktidController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProduktidController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produkt>>> GetProduktid()
        {
            return await _context.Produktid.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produkt>> GetProdukt(int id)
        {
            var produkt = await _context.Produktid.FindAsync(id);
            if (produkt == null) return NotFound();
            return produkt;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProdukt(int id)
        {
            var produkt = await _context.Produktid.FindAsync(id);
            if (produkt == null) return NotFound();
            _context.Produktid.Remove(produkt);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}