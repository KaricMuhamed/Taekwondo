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
    public class ObjaveController : ControllerBase
    {
        private readonly IObjavaService _objavaService;

        public ObjaveController(IObjavaService objavaService)
        {
            _objavaService = objavaService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ObjavaResponseDto>>> GetObjave()
        {
            try
            {
                var objave = await _objavaService.GetAllAsync();
                return Ok(objave);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja objava." });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ObjavaResponseDto>> GetObjava(Guid id)
        {
            try
            {
                var objava = await _objavaService.GetByIdAsync(id);
                if (objava == null)
                    return NotFound(new { message = "Objava nije pronadena." });

                return Ok(objava);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja objave." });
            }
        }

        [HttpGet("autor/{autorId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ObjavaResponseDto>>> GetObjaveByAutor(Guid autorId)
        {
            try
            {
                var objave = await _objavaService.GetByAutorAsync(autorId);
                return Ok(objave);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja objava." });
            }
        }

        [HttpGet("tip/{tip}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ObjavaResponseDto>>> GetObjaveByTip(string tip)
        {
            try
            {
                var objave = await _objavaService.GetByTipAsync(tip);
                return Ok(objave);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja objava." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "administrator,trener")]
        public async Task<ActionResult<ObjavaResponseDto>> PostObjava(ObjavaCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Set author to current user
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized();

                dto.AutorId = Guid.Parse(currentUserId);

                var objava = await _objavaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetObjava), new { id = objava.Id }, objava);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom kreiranja objave." });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ObjavaResponseDto>> PutObjava(Guid id, ObjavaCreateDto dto)
        {
            try
            {
                // Check if objava exists first
                var existingObjava = await _objavaService.GetByIdAsync(id);
                if (existingObjava == null)
                    return NotFound(new { message = "Objava nije pronadena." });

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Only author or admin can edit
                if (currentUserRole != "administrator" &&
                    existingObjava.AutorId.ToString() != currentUserId)
                {
                    return Forbid();
                }

                // Keep the original author
                dto.AutorId = existingObjava.AutorId;

                var objava = await _objavaService.UpdateAsync(id, dto);
                return Ok(objava);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom azuriranja objave." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteObjava(Guid id)
        {
            try
            {
                // Check if objava exists first
                var existingObjava = await _objavaService.GetByIdAsync(id);
                if (existingObjava == null)
                    return NotFound(new { message = "Objava nije pronadena." });

                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // Only author or admin can delete
                if (currentUserRole != "administrator" &&
                    existingObjava.AutorId.ToString() != currentUserId)
                {
                    return Forbid();
                }

                var result = await _objavaService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom brisanja objave." });
            }
        }

        [HttpGet("moje")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ObjavaResponseDto>>> GetMyObjave()
        {
            try
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                    return Unauthorized();

                var objave = await _objavaService.GetByAutorAsync(Guid.Parse(currentUserId));
                return Ok(objave);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Greska prilikom dohvatanja objava." });
            }
        }
    }
}