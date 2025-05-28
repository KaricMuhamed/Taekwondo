using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController(TaekwondoDbContext context) : ControllerBase
{
    private readonly TaekwondoDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
    {
        return Ok(await _context.Payments
            .Include(p => p.Member)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync());
    }

    [HttpGet("member/{memberId}")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByMember(int memberId)
    {
        var exists = await _context.Members.AnyAsync(m => m.Id == memberId);
        if (!exists) return NotFound("Member not found.");

        var payments = await _context.Payments
            .Where(p => p.MemberId == memberId)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        return Ok(payments);
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment(PaymentDto dto)
    {
        var member = await _context.Members.FindAsync(dto.MemberId);
        if (member == null) return BadRequest("Member not found.");

        var payment = new Payment
        {
            MemberId = dto.MemberId,
            PaymentDate = dto.PaymentDate,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod,
            Notes = dto.Notes,
            PaymentMonth = dto.PaymentMonth,
            PaymentYear = dto.PaymentYear
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPaymentsByMember), new { memberId = dto.MemberId }, payment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
