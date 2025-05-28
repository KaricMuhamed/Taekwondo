using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

[Route("api/[controller]")]
[ApiController]
public class TournamentsController : ControllerBase
{
    private readonly TaekwondoDbContext _context;

    public TournamentsController(TaekwondoDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TournamentDto dto)
    {
        if (dto == null)
            return BadRequest(new { error = "Request body is required." });

        if (!dto.GroupId.HasValue && (dto.Members == null || !dto.Members.Any()))
        {
            return BadRequest(new { error = "Either GroupId or Members must be provided." });
        }

        var members = new List<Members>();

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

        var tournament = new Tournament
        {
            Name = dto.Name,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Location = dto.Location,
            Description = dto.Description,
            GroupId = dto.GroupId,
            TournamentMembers = members.Select(m => new TournamentMember
                {
                    MemberId = m.Id
                }).ToList()
        };


        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();

        var responseDto = new TournamentDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Description = tournament.Description,
            GroupId = tournament.GroupId,
            Members = tournament.TournamentMembers?
                .Where(tm => tm.Member != null)
                .Select(tm => new MemberDto
                {
                    Id = tm.Member.Id,
                    Name = tm.Member.Name
                }).ToList()

    };

        return CreatedAtAction(nameof(GetById), new { id = tournament.Id }, responseDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tournaments = await _context.Tournaments
            .Include(t => t.Group)
            .Include(t => t.TournamentMembers)
                .ThenInclude(tm => tm.Member)
            .ToListAsync();

        var responseDtos = tournaments.Select(tournament => new TournamentDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Description = tournament.Description,
            GroupId = tournament.GroupId,
            Members = tournament.TournamentMembers?
                .Where(tm => tm.Member != null)
                .Select(tm => new MemberDto
                {
                    Id = tm.Member.Id,
                    Name = tm.Member.Name
                }).ToList()
        }).ToList();

        return Ok(responseDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tournament = await _context.Tournaments
            .Include(t => t.Group)
            .Include(t => t.TournamentMembers)
                .ThenInclude(tm => tm.Member)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tournament == null)
            return NotFound();

        var responseDto = new TournamentDto
        {
            Id = tournament.Id,
            Name = tournament.Name,
            StartDate = tournament.StartDate,
            EndDate = tournament.EndDate,
            Location = tournament.Location,
            Description = tournament.Description,
            GroupId = tournament.GroupId,
            Members = tournament.TournamentMembers?
                .Where(tm => tm.Member != null)
                .Select(tm => new MemberDto
                {
                    Id = tm.Member.Id,
                    Name = tm.Member.Name
                }).ToList()
        };

        return Ok(responseDto);
    }

    [HttpGet("bydate")]
    public async Task<IActionResult> GetByDate([FromQuery] DateTime date)
    {
        var tournaments = await _context.Tournaments
            .Include(t => t.Group)
            .Include(t => t.TournamentMembers)
                .ThenInclude(tm => tm.Member)
            .Where(t => date >= t.StartDate && date <= t.EndDate)
            .ToListAsync();

        if (tournaments == null || tournaments.Count == 0)
            return NotFound(new { error = $"No tournaments found on date {date.ToShortDateString()}" });

        var responseDtos = tournaments.Select(t => new TournamentDto
        {
            Id = t.Id,
            Name = t.Name,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            Location = t.Location,
            Description = t.Description,
            GroupId = t.GroupId,
            Members = t.TournamentMembers?
                .Where(tm => tm.Member != null)
                .Select(tm => new MemberDto
                {
                    Id = tm.Member.Id,
                    Name = tm.Member.Name
                }).ToList() ?? new List<MemberDto>()
        }).ToList();

        return Ok(responseDtos);
    }

}
