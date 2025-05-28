using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly TaekwondoDbContext _context;

        public GroupController(TaekwondoDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetGroups()
        {
            var groups = await _context.Groups
                .Include(g => g.GroupMembers)
                .ToListAsync();

            var result = groups.Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                MemberIds = g.GroupMembers?.Select(m => m.MemberId).ToList() ?? new()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDto>> GetGroup(int id)
        {
            var group = await _context.Groups
                .Include(g => g.GroupMembers)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return NotFound();

            var result = new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                MemberIds = group.GroupMembers?.Select(m => m.MemberId).ToList() ?? new()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDto groupDto)
        {
            var group = new Group
            {
                Name = groupDto.Name,
                GroupMembers = new List<GroupMember>()
            };

            var existingMembers = await _context.Members
                .Where(m => groupDto.MemberIds.Contains(m.Id))
                .ToListAsync();

            var missingMemberIds = groupDto.MemberIds.Except(existingMembers.Select(m => m.Id)).ToList();
            if (missingMemberIds.Any())
            {
                return BadRequest($"Members with IDs {string.Join(", ", missingMemberIds)} do not exist.");
            }

            foreach (var member in existingMembers)
            {
                group.GroupMembers.Add(new GroupMember
                {
                    MemberId = member.Id
                });
            }

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return Ok(group);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] UpdateGroupDto dto)
        {
            var group = await _context.Groups
                .Include(g => g.GroupMembers)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
                return NotFound();

            group.Name = dto.Name;

            group.GroupMembers?.Clear();

            if (dto.MemberIds != null)
            {
                group.GroupMembers = dto.MemberIds.Select(mid => new GroupMember
                {
                    GroupId = group.Id,
                    MemberId = mid
                }).ToList();
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound();

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{groupId}/add-member/{memberId}")]
        public async Task<IActionResult> AddMemberToGroup(int groupId, int memberId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            var member = await _context.Members.FindAsync(memberId);

            if (group == null || member == null)
                return NotFound();

            var exists = await _context.GroupMembers.AnyAsync(gm =>
                gm.GroupId == groupId && gm.MemberId == memberId);

            if (!exists)
            {
                _context.GroupMembers.Add(new GroupMember
                {
                    GroupId = groupId,
                    MemberId = memberId
                });
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpDelete("{groupId}/remove-member/{memberId}")]
        public async Task<IActionResult> RemoveMemberFromGroup(int groupId, int memberId)
        {
            var entry = await _context.GroupMembers.FirstOrDefaultAsync(gm =>
                gm.GroupId == groupId && gm.MemberId == memberId);

            if (entry == null)
                return NotFound();

            _context.GroupMembers.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
