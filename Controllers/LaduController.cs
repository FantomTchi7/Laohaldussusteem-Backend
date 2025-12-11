using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaduController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LaduController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ladu>>> GetLaod()
        {
            return await _context.Laod.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ladu>> GetLadu(int id)
        {
            var ladu = await _context.Laod.FindAsync(id);

            if (ladu == null)
            {
                return NotFound();
            }

            return ladu;
        }

        [HttpPost]
        public async Task<ActionResult<Ladu>> PostLadu(Ladu ladu)
        {
            _context.Laod.Add(ladu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLadu", new { id = ladu.Id }, ladu);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLadu(int id, Ladu ladu)
        {
            if (id != ladu.Id)
            {
                return BadRequest();
            }

            _context.Entry(ladu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaduExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLadu(int id)
        {
            var ladu = await _context.Laod.FindAsync(id);
            if (ladu == null)
            {
                return NotFound();
            }

            _context.Laod.Remove(ladu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaduExists(int id)
        {
            return _context.Laod.Any(e => e.Id == id);
        }
    }
}