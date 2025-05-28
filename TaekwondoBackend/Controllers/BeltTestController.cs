using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

[Route("api/[controller]")]
[ApiController]
public class BeltTestsController : ControllerBase
{
    private readonly TaekwondoDbContext _context;

    public BeltTestsController(TaekwondoDbContext context)
    {
        _context = context;
    }

    // Get all belt tests
    [HttpGet]
    public async Task<ActionResult<List<BeltTestDto>>> GetAll()
    {
        var tests = await _context.BeltTests
            .Include(bt => bt.Member)
            .Include(bt => bt.CurrentBelt)
            .Include(bt => bt.AppliedBelt)
            .ToListAsync();

        var dtos = tests.Select(bt => new BeltTestDto
        {
            Id = bt.Id,
            MemberId = bt.MemberId,
            MemberName = bt.Member.Name,
            CurrentBeltId = bt.CurrentBeltId,
            CurrentBeltName = bt.CurrentBelt.Name,
            AppliedBeltId = bt.AppliedBeltId,
            AppliedBeltName = bt.AppliedBelt.Name,
            ScheduledDate = bt.ScheduledDate,
            TestedDate = bt.TestedDate,
            Status = bt.Status.ToString(),
            Notes = bt.Notes
        }).ToList();

        return Ok(dtos);
    }

    // Get belt test by id
    [HttpGet("{id}")]
    public async Task<ActionResult<BeltTestDto>> GetById(int id)
    {
        var bt = await _context.BeltTests
            .Include(b => b.Member)
            .Include(b => b.CurrentBelt)
            .Include(b => b.AppliedBelt)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (bt == null)
            return NotFound();

        var dto = new BeltTestDto
        {
            Id = bt.Id,
            MemberId = bt.MemberId,
            MemberName = bt.Member.Name,
            CurrentBeltId = bt.CurrentBeltId,
            CurrentBeltName = bt.CurrentBelt.Name,
            AppliedBeltId = bt.AppliedBeltId,
            AppliedBeltName = bt.AppliedBelt.Name,
            ScheduledDate = bt.ScheduledDate,
            TestedDate = bt.TestedDate,
            Status = bt.Status.ToString(),
            Notes = bt.Notes
        };

        return Ok(dto);
    }

    // Create a new belt test record (schedule)
    [HttpPost]
    public async Task<ActionResult<BeltTestDto>> Create(BeltTestCreateDto dto)
    {
        var member = await _context.Members.FindAsync(dto.MemberId);
        if (member == null) return BadRequest(new { error = "Member not found." });

        var appliedBelt = await _context.Belts.FindAsync(dto.AppliedBeltId);
        if (appliedBelt == null) return BadRequest(new { error = "Applied belt not found." });

        var currentBelt = await _context.Belts.FindAsync(member.CurrentBeltId);
        if (currentBelt == null) return BadRequest(new { error = "Member's current belt not found." });

        var test = new BeltTest
        {
            MemberId = dto.MemberId,
            AppliedBeltId = dto.AppliedBeltId,
            CurrentBeltId = member.CurrentBeltId,
            ScheduledDate = dto.ScheduledDate,
            Status = BeltTestStatus.Scheduled,
            Notes = dto.Notes
        };

        _context.BeltTests.Add(test);
        await _context.SaveChangesAsync();

        // Return created test as DTO
        var resultDto = new BeltTestDto
        {
            Id = test.Id,
            MemberId = member.Id,
            MemberName = member.Name,
            CurrentBeltId = currentBelt.Id,
            CurrentBeltName = currentBelt.Name,
            AppliedBeltId = appliedBelt.Id,
            AppliedBeltName = appliedBelt.Name,
            ScheduledDate = test.ScheduledDate,
            TestedDate = null,
            Status = test.Status.ToString(),
            Notes = test.Notes
        };

        return CreatedAtAction(nameof(GetById), new { id = test.Id }, resultDto);
    }

    // Update test result / status
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, BeltTestUpdateDto dto)
    {
        var test = await _context.BeltTests
            .Include(t => t.Member)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (test == null)
            return NotFound();

        // Validate status
        if (!Enum.TryParse<BeltTestStatus>(dto.Status, true, out var status))
            return BadRequest(new { error = "Invalid status value." });

        test.Status = status;
        if (dto.TestedDate.HasValue)
            test.TestedDate = dto.TestedDate.Value;

        test.Notes = dto.Notes;

        // If passed, update member current belt
        if (status == BeltTestStatus.Passed)
        {
            // Promote member belt to AppliedBelt
            test.Member.CurrentBeltId = test.AppliedBeltId;
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var test = await _context.BeltTests.FindAsync(id);
        if (test == null)
            return NotFound();

        _context.BeltTests.Remove(test);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
