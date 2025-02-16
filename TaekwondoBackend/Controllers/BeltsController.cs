using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeltsController(TaekwondoDbContext context) : ControllerBase
    {
        private readonly TaekwondoDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Belts>>> GetBelts()
        {
            return Ok(await _context.Belts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Belts>> GetBeltsByIdAsync(int id)
        {
            var belt = await _context.Belts.FindAsync(id);

            if (belt == null)
                return NotFound();

            return Ok(belt);
        }

        [HttpPost]
        public async Task<ActionResult<Belts>> AddBelt(Belts newBelt)
        {
            if (newBelt == null)
                return BadRequest();

            _context.Belts.Add(newBelt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBeltsByIdAsync),
                new { id = newBelt.Id },
                newBelt
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBelt(int id, Belts updatedBelt)
        {
            var belt = await _context.Belts.FindAsync(id);

            if (belt == null)
                return NotFound();

            belt.Name = updatedBelt.Name;
            belt.SequenceNumber = updatedBelt.SequenceNumber;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeltAsync(int id)
        {
            var belt = await _context.Belts.FindAsync(id);
            if (belt == null)
                return NotFound();

            _context.Belts.Remove(belt);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
