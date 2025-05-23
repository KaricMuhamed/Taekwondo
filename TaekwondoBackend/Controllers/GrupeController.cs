using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaekwondoBackend.Models;
using TaekwondoBackend.Services;

namespace TaekwondoBackend.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class GrupeController : ControllerBase
        {
            private readonly IGrupaService _grupaService;

            public GrupeController(IGrupaService grupaService)
            {
                _grupaService = grupaService;
            }

            [HttpGet]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<IEnumerable<GrupaResponseDto>>> GetGrupe()
            {
            try
            {
                    var grupe = await _grupaService.GetAllAsync();
                    return Ok(grupe);
                }
                catch (Exception)
            {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja grupa." });
                }
            }

            [HttpGet("{id}")]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<GrupaResponseDto>> GetGrupa(Guid id)
            {
                try
                {
                    var grupa = await _grupaService.GetByIdAsync(id);
                    if (grupa == null)
                        return NotFound(new { message = "Grupa nije pronadena." });

                    return Ok(grupa);
                }
                catch (Exception)
            {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja grupe." });
                }
            }

            [HttpPost]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<GrupaResponseDto>> PostGrupa(GrupaCreateDto dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var grupa = await _grupaService.CreateAsync(dto);
                    return CreatedAtAction(nameof(GetGrupa), new { id = grupa.Id }, grupa);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom kreiranja grupe." });
                }
            }

            [HttpPut("{id}")]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<GrupaResponseDto>> PutGrupa(Guid id, GrupaCreateDto dto)
            {
                try
                {
                    var grupa = await _grupaService.UpdateAsync(id, dto);
                    if (grupa == null)
                        return NotFound(new { message = "Grupa nije pronadena." });

                    return Ok(grupa);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom azuriranja grupe." });
                }
            }

            [HttpDelete("{id}")]
            [Authorize(Roles = "administrator")]
            public async Task<IActionResult> DeleteGrupa(Guid id)
            {
                try
                {
                    var result = await _grupaService.DeleteAsync(id);
                    if (!result)
                        return NotFound(new { message = "Grupa nije pronadena." });

                    return NoContent();
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom brisanja grupe." });
                }
            }

            [HttpPost("{grupaId}/ucenici/{ucenikId}")]
            [Authorize(Roles = "administrator,trener")]
            public async Task<IActionResult> AddUcenikToGrupa(Guid grupaId, Guid ucenikId)
            {
                try
                {
                    var result = await _grupaService.AddUcenikToGrupaAsync(grupaId, ucenikId);
                    if (!result)
                        return BadRequest(new { message = "Ucenik je vec u grupi ili grupa/ucenik ne postoji." });

                    return Ok(new { message = "Ucenik je uspjesno dodan u grupu." });
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom dodavanja ucenika u grupu." });
                }
            }

            [HttpDelete("{grupaId}/ucenici/{ucenikId}")]
            [Authorize(Roles = "administrator,trener")]
            public async Task<IActionResult> RemoveUcenikFromGrupa(Guid grupaId, Guid ucenikId)
            {
                try
                {
                    var result = await _grupaService.RemoveUcenikFromGrupaAsync(grupaId, ucenikId);
                    if (!result)
                        return NotFound(new { message = "Ucenik nije u grupi." });

                    return Ok(new { message = "Ucenik je uspjesno uklonjen iz grupe." });
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom uklanjanja ucenika iz grupe." });
                }
            }

            [HttpGet("{grupaId}/ucenici")]
            [Authorize(Roles = "administrator,trener")]
            public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetUceniciInGrupa(Guid grupaId)
            {
                try
                {
                    var ucenici = await _grupaService.GetUceniciInGrupaAsync(grupaId);
                    return Ok(ucenici);
                }
                catch (Exception)
                {
                    return StatusCode(500, new { message = "Greska prilikom dohvatanja ucenika u grupi." });
                }
            }
        }
    }
