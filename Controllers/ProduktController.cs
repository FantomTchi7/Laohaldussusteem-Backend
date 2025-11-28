using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduktController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProduktController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produkt>>> GetProduktid()
        {
            return await _context.Produktid
                .Where(p => !(p is Toode))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produkt>> GetProdukt(int id)
        {
            var produkt = await _context.Produktid.FindAsync(id);
            if (produkt == null) return NotFound();
            return produkt;
        }

        [HttpPost]
        public async Task<ActionResult<Produkt>> PostProdukt(Produkt produkt)
        {
            _context.Produktid.Add(produkt);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProdukt", new { id = produkt.Id }, produkt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProdukt(int id, Produkt produkt)
        {
            if (id != produkt.Id) return BadRequest();
            _context.Entry(produkt).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
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