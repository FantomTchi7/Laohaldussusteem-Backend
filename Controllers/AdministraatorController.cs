using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Helpers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministraatorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdministraatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administraator>>> GetAdministraatorid()
        {
            return await _context.Administraatorid.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Administraator>> GetAdministraator(int id)
        {
            var admin = await _context.Administraatorid.FindAsync(id);
            if (admin == null) return NotFound();
            return admin;
        }

        [HttpPost]
        public async Task<ActionResult<Administraator>> PostAdministraator(Administraator admin)
        {
            if (await _context.Kasutajad.AnyAsync(u => u.Email == admin.Email))
            {
                return BadRequest("Email on juba kasutusel.");
            }

            admin.KasutajaTüüp = KasutajaTüüp.Administraator;
            admin.Parool = ParoolHelper.HashPassword(admin.Parool);

            _context.Administraatorid.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdministraator", new { id = admin.Id }, admin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdministraator(int id, Administraator updatedAdmin)
        {
            if (id != updatedAdmin.Id) return BadRequest();

            var existingAdmin = await _context.Administraatorid.FindAsync(id);
            if (existingAdmin == null) return NotFound();

            existingAdmin.Nimi = updatedAdmin.Nimi;
            existingAdmin.Email = updatedAdmin.Email;

            if (!string.IsNullOrEmpty(updatedAdmin.Parool))
            {
                existingAdmin.Parool = ParoolHelper.HashPassword(updatedAdmin.Parool);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministraator(int id)
        {
            var admin = await _context.Administraatorid.FindAsync(id);
            if (admin == null) return NotFound();

            _context.Administraatorid.Remove(admin);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}