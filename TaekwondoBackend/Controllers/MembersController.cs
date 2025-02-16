using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController(TaekwondoDbContext context) : ControllerBase
    {
        private readonly TaekwondoDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<MembersDto>>> GetMembers()
        {
            return Ok(await _context.Members.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MembersDto>> GetMembersByIdAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
                return NotFound();

            return Ok(member);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<MembersDto>> AddMember(MembersDto newMember)
        {
            if (newMember == null)
                return BadRequest();

            _context.Members.Add(newMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction( 
                nameof(GetMembersByIdAsync),
                new { id = newMember.Id },
                newMember
                );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, MembersDto updatedMember)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
                return NotFound();

            member.Name = member.Name;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberAsync(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound();

            _context.Members.Remove(member);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
