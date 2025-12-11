using backend.Data;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laohaldussusteem.Server.Controllers
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Parool { get; set; }
    }

    public class RegisterRequest
    {
        public string Eesnimi { get; set; }
        public string Perenimi { get; set; }
        public string Email { get; set; }
        public string Parool { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Kasutaja>> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Kasutajad
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("Vale email või parool.");
            }

            bool isPasswordValid = ParoolHelper.VerifyPassword(request.Parool, user.Parool);

            if (!isPasswordValid)
            {
                return Unauthorized("Vale email või parool.");
            }

            user.Parool = null;
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // 1. Check if email already exists
            if (await _context.Kasutajad.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("See email on juba kasutusel.");
            }

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Parool))
            {
                return BadRequest("Email ja parool on kohustuslikud.");
            }

            var newUser = new Tellija
            {
                Nimi = $"{request.Eesnimi} {request.Perenimi}".Trim(),
                Email = request.Email,
                Parool = ParoolHelper.HashPassword(request.Parool),
                KasutajaTüüp = KasutajaTüüp.Tellija
            };

            _context.Tellijad.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Konto loodud edukalt");
        }
    }
}