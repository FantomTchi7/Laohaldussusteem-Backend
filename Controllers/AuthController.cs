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
    }
}