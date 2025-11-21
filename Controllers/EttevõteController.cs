using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EttevõteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EttevõteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ettevõte>>> GetEttevõtted()
        {
            return await _context.Ettevõtted.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ettevõte>> GetEttevõte(int id)
        {
            var ettevõte = await _context.Ettevõtted.FindAsync(id);
            if (ettevõte == null) return NotFound();
            return ettevõte;
        }

        [HttpPost]
        public async Task<ActionResult<Ettevõte>> PostEttevõte(Ettevõte ettevõte)
        {
            _context.Ettevõtted.Add(ettevõte);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEttevõte", new { id = ettevõte.Id }, ettevõte);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEttevõte(int id, Ettevõte ettevõte)
        {
            if (id != ettevõte.Id) return BadRequest();
            _context.Entry(ettevõte).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEttevõte(int id)
        {
            var ettevõte = await _context.Ettevõtted.FindAsync(id);
            if (ettevõte == null) return NotFound();
            _context.Ettevõtted.Remove(ettevõte);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}