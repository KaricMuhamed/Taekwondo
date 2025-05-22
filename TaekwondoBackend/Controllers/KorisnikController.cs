using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;
using TaekwondoBackend.Services;

namespace TaekwondoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KorisniciController : ControllerBase
    {
        private readonly IKorisnikService _korisnikService;

        public KorisniciController(IKorisnikService korisnikService)
        {
            _korisnikService = korisnikService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new LoginResponseDto
                {
                    Success = false,
                    Message = "Email i password su obavezni.",
                    Korisnik = null,
                    Token = null
                });
            }

            var result = await _korisnikService.LoginAsync(loginDto);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized(result);
            }
        }

        [HttpGet]
        [Authorize(Roles = "administrator,trener")]
        public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetKorisnici()
        {
            try
            {
                var korisnici = await _korisnikService.GetAllAsync();
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja korisnika." });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<KorisnikResponseDto>> GetKorisnik(Guid id)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only view themselves unless they're admin or trener
                if (currentUserRole != "administrator" &&
                    currentUserRole != "trener" &&
                    id.ToString() != currentUserId)
                {
                    return Forbid();
                }

                var korisnik = await _korisnikService.GetByIdAsync(id);
                if (korisnik == null)
                    return NotFound(new { message = "Korisnik nije pronaden." });

                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja korisnika." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<KorisnikResponseDto>> PostKorisnik(KorisnikCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var korisnik = await _korisnikService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetKorisnik), new { id = korisnik.Id }, korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom kreiranja korisnika." });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<KorisnikResponseDto>> PutKorisnik(Guid id, KorisnikCreateDto dto)
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Users can only edit themselves, unless they're admin
                if (currentUserRole != "administrator" && id.ToString() != currentUserId)
                {
                    return Forbid();
                }

                var korisnik = await _korisnikService.UpdateAsync(id, dto);
                if (korisnik == null)
                    return NotFound(new { message = "Korisnik nije pronaden." });

                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom azuriranja korisnika." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> DeleteKorisnik(Guid id)
        {
            try
            {
                var result = await _korisnikService.DeleteAsync(id);
                if (!result)
                    return NotFound(new { message = "Korisnik nije pronaden." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom brisanja korisnika." });
            }
        }

        [HttpGet("profil")]
        [Authorize]
        public async Task<ActionResult<KorisnikResponseDto>> GetMyProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var korisnik = await _korisnikService.GetByIdAsync(Guid.Parse(userId));
                if (korisnik == null)
                    return NotFound();

                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja profila." });
            }
        }

        [HttpGet("uloga/{uloga}")]
        [Authorize(Roles = "administrator,trener")]
        public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetKorisniciByUloga(string uloga)
        {
            try
            {
                var korisnici = await _korisnikService.GetByUlogaAsync(uloga);
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja korisnika." });
            }
        }

        [HttpGet("pojas/{pojas}")]
        [Authorize(Roles = "administrator,trener")]
        public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetKorisniciByPojas(string pojas)
        {
            try
            {
                var korisnici = await _korisnikService.GetByPojasAsync(pojas);
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja korisnika." });
            }
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<KorisnikResponseDto>> GetKorisnikByEmail(string email)
        {
            try
            {
                var korisnik = await _korisnikService.GetByEmailAsync(email);
                if (korisnik == null)
                    return NotFound(new { message = "Korisnik nije pronaden." });

                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja korisnika." });
            }
        }

        [HttpGet("aktivni")]
        [Authorize(Roles = "administrator,trener")]
        public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetAktivniKorisnici()
        {
            try
            {
                var korisnici = await _korisnikService.GetActiveAsync();
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja korisnika." });
            }
        }

        [HttpGet("treneri")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetTreneri()
        {
            try
            {
                var korisnici = await _korisnikService.GetTreneriAsync();
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja trenera." });
            }
        }

        [HttpGet("ucenici")]
        [Authorize(Roles = "administrator,trener")]
        public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetUcenici()
        {
            try
            {
                var korisnici = await _korisnikService.GetUceniciAsync();
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja ucenika." });
            }
        }

        [HttpGet("minimalni-pojas/{pojas}")]
        [Authorize(Roles = "administrator,trener")]
        public async Task<ActionResult<IEnumerable<KorisnikResponseDto>>> GetByMinimumBelt(string pojas)
        {
            try
            {
                var korisnici = await _korisnikService.GetByMinimumBeltAsync(pojas);
                return Ok(korisnici);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja korisnika." });
            }
        }
    }
}
