using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaekwondoBackend.Models;
using TaekwondoBackend.Services;

namespace TaekwondoBackend.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class TreninziController : ControllerBase
        {
            private readonly ITreningService _treningService;

            public TreninziController(ITreningService treningService)
            {
                _treningService = treningService;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<TreningResponseDto>>> GetTreninzi()
            {
                try
                {
                    var treninzi = await _treningService.GetAllAsync();
                    return Ok(treninzi);
                }
                catch (Exception)   
            {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja treninga." });
                }
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<TreningResponseDto>> GetTrening(Guid id)
            {
            try
            {
                    var trening = await _treningService.GetByIdAsync(id);
                    if (trening == null)
                        return NotFound(new { message = "Trening nije pronaden." });

                    return Ok(trening);
                }
                catch (Exception)
            {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja treninga." });
                }
            }

            [HttpPost]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<TreningResponseDto>> PostTrening(TreningCreateDto dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    // Validate time logic
                    if (dto.VrijemeOd >= dto.VrijemeDo)
                        return BadRequest(new { message = "Vrijeme pocetka mora biti prije vremena zavrsetka." });

                    var trening = await _treningService.CreateAsync(dto);
                    return CreatedAtAction(nameof(GetTrening), new { id = trening.Id }, trening);
                }
                catch (Exception)
            {
                    return StatusCode(500, new { message = "Greska prilikom kreiranja treninga." });
                }
            }

            [HttpPut("{id}")]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<TreningResponseDto>> PutTrening(Guid id, TreningCreateDto dto)
            {
                try
                {
                    // Validate time logic
                    if (dto.VrijemeOd >= dto.VrijemeDo)
                        return BadRequest(new { message = "Vrijeme pocetka mora biti prije vremena zavrsetka." });

                    var trening = await _treningService.UpdateAsync(id, dto);
                    if (trening == null)
                        return NotFound(new { message = "Trening nije pronaden." });

                    return Ok(trening);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom azuriranja treninga." });
                }
            }

            [HttpDelete("{id}")]
            [Authorize(Roles = "administrator")]
            public async Task<IActionResult> DeleteTrening(Guid id)
            {
                try
                {
                    var result = await _treningService.DeleteAsync(id);
                    if (!result)
                        return NotFound(new { message = "Trening nije pronaden." });

                    return NoContent();
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom brisanja treninga." });
                }
            }

            [HttpGet("datum/{datum}")]
            public async Task<ActionResult<IEnumerable<TreningResponseDto>>> GetTreninziByDate(DateTime datum)
            {
                try
                {
                    var treninzi = await _treningService.GetTreninziByDateAsync(datum);
                    return Ok(treninzi);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja treninga." });
                }
            }

            [HttpGet("trener/{trenerId}")]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<IEnumerable<TreningResponseDto>>> GetTreninziByTrener(Guid trenerId)
            {
                try
                {
                    var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                    // Users can only view their own trainings unless they're admin
                    if (currentUserRole != "administrator" && trenerId.ToString() != currentUserId)
                    {
                        return Forbid();
                    }

                    var treninzi = await _treningService.GetTreninziByTrenerAsync(trenerId);
                    return Ok(treninzi);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja treninga." });
                }
            }

            [HttpGet("grupa/{grupaId}")]
            public async Task<ActionResult<IEnumerable<TreningResponseDto>>> GetTreninziByGrupa(Guid grupaId)
            {
                try
                {
                    var treninzi = await _treningService.GetTreninziByGrupaAsync(grupaId);
                    return Ok(treninzi);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja treninga." });
                }
            }

            [HttpGet("nadolazeci")]
            public async Task<ActionResult<IEnumerable<TreningResponseDto>>> GetUpcomingTreninzi()
            {
                try
                {
                    var treninzi = await _treningService.GetUpcomingTreninziAsync();
                    return Ok(treninzi);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja nadolazecih treninga." });
                }
            }

            [HttpGet("moji")]
            [Authorize(Roles = "trener")]
            public async Task<ActionResult<IEnumerable<TreningResponseDto>>> GetMyTreninzi()
            {
                try
                {
                    var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(currentUserId))
                        return Unauthorized();

                    var treninzi = await _treningService.GetTreninziByTrenerAsync(Guid.Parse(currentUserId));
                    return Ok(treninzi);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja vasih treninga." });
                }
            }
        }
    }
