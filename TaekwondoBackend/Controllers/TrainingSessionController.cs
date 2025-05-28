using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

[Route("api/[controller]")]
[ApiController]
public class TrainingSessionsController : ControllerBase
{
    private readonly TaekwondoDbContext _context;

    public TrainingSessionsController(TaekwondoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TrainingSessionDto dto)
    {
        if (dto == null)
            return BadRequest(new { error = "Request body is required." });

        if (!Guid.TryParse(dto.InstructorId, out var instructorGuid))
            return BadRequest(new { error = "Invalid instructor ID format." });

        var instructor = await _context.Users.FindAsync(instructorGuid);
        if (instructor == null)
            return BadRequest(new { error = "Instructor not found." });

        var members = new List<Members>();

        if (!dto.GroupId.HasValue && (dto.Members == null || !dto.Members.Any()))
        {
            return BadRequest(new { error = "Either GroupId or Members must be provided." });
        }

        if (dto.GroupId.HasValue)
        {
            var group = await _context.Groups
                .Include(g => g.GroupMembers!)
                    .ThenInclude(gm => gm.Member)
                .FirstOrDefaultAsync(g => g.Id == dto.GroupId.Value);

            if (group == null)
                return BadRequest(new { error = "Group not found." });

            var groupMembers = group.GroupMembers?
                .Where(gm => gm.Member != null)
                .Select(gm => gm.Member!)
                .ToList() ?? new List<Members>();

            members.AddRange(groupMembers);
        }

        if (dto.Members != null && dto.Members.Any())
        {
            var memberIds = dto.Members.Select(m => m.Id).ToList();
            var individualMembers = await _context.Members
                .Where(m => memberIds.Contains(m.Id))
                .ToListAsync();

            var missingIds = memberIds.Except(individualMembers.Select(m => m.Id)).ToList();
            if (missingIds.Any())
                return BadRequest(new { error = $"Missing members: {string.Join(", ", missingIds)}" });

            var existingMemberIds = members.Select(m => m.Id).ToHashSet();
            var newMembers = individualMembers.Where(m => !existingMemberIds.Contains(m.Id));
            members.AddRange(newMembers);
        }

        var session = new TrainingSession
        {
            Date = dto.Date,
            Location = dto.Location,
            Description = dto.Description,
            InstructorId = instructor.Id,
            GroupId = dto.GroupId,
            TrainingSessionMembers = members.Select(m => new TrainingSessionMember
            {
                MemberId = m.Id
            }).ToList()
        };

        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var responseDto = new TrainingSessionDto
        {
            Id = session.Id,
            Date = session.Date,
            Location = session.Location,
            Description = session.Description,
            InstructorId = instructor.Id.ToString(),
            Instructor = new InstructorDto
            {
                Id = instructor.Id.ToString(),
                Username = instructor.Username,
                Email = instructor.Email,
                Role = instructor.Role
            },
            GroupId = session.GroupId,
            Members = members.Select(m => new MemberDto
            {
                Id = m.Id,
                Name = m.Name
            }).ToList()
        };

        return CreatedAtAction(nameof(GetById), new { id = session.Id }, responseDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sessions = await _context.TrainingSessions
            .Include(ts => ts.Instructor)
            .Include(ts => ts.TrainingSessionMembers)
                .ThenInclude(tsm => tsm.Member)
            .ToListAsync();

        var responseDtos = sessions.Select(session => new TrainingSessionDto
        {
            Id = session.Id,
            Date = session.Date,
            Location = session.Location,
            Description = session.Description,
            InstructorId = session.Instructor?.Id.ToString() ?? string.Empty,
            Instructor = session.Instructor != null ? new InstructorDto
            {
                Id = session.Instructor.Id.ToString(),
                Username = session.Instructor.Username,
                Email = session.Instructor.Email,
                Role = session.Instructor.Role
            } : null,
            GroupId = session.GroupId,
            Members = session.TrainingSessionMembers?
                .Where(tsm => tsm.Member != null)
                .Select(tsm => new MemberDto
                {
                    Id = tsm.Member!.Id,
                    Name = tsm.Member.Name
                }).ToList() ?? new List<MemberDto>()
        }).ToList();

        return Ok(responseDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var session = await _context.TrainingSessions
            .Include(ts => ts.Instructor)
            .Include(ts => ts.TrainingSessionMembers)
                .ThenInclude(tsm => tsm.Member)
            .FirstOrDefaultAsync(ts => ts.Id == id);

        if (session == null)
            return NotFound();

        var responseDto = new TrainingSessionDto
        {
            Id = session.Id,
            Date = session.Date,
            Location = session.Location,
            Description = session.Description,
            InstructorId = session.Instructor?.Id.ToString() ?? string.Empty,
            Instructor = session.Instructor != null ? new InstructorDto
            {
                Id = session.Instructor.Id.ToString(),
                Username = session.Instructor.Username,
                Email = session.Instructor.Email,
                Role = session.Instructor.Role
            } : null,
            GroupId = session.GroupId,
            Members = session.TrainingSessionMembers?
                .Where(tsm => tsm.Member != null)
                .Select(tsm => new MemberDto
                {
                    Id = tsm.Member!.Id,
                    Name = tsm.Member.Name
                }).ToList() ?? new List<MemberDto>()
        };

        return Ok(responseDto);
    }
}
