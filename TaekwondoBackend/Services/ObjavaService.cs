using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{
    public class ObjavaService : IObjavaService
    {
        private readonly ApplicationDbContext _context;

        public ObjavaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ObjavaResponseDto>> GetAllAsync()
        {
            var objave = await _context.Objave
                .Include(o => o.Autor)
                .ToListAsync();

            return objave.Select(o => new ObjavaResponseDto
            {
                Id = o.Id,
                Naslov = o.Naslov,
                Sadrzaj = o.Sadrzaj,
                Tip = o.Tip,
                ImageUrl = o.ImageUrl,
                AutorId = o.AutorId,
                AutorIme = o.Autor.Ime,
                Objavljeno = o.Objavljeno,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            });
        }

        public async Task<ObjavaResponseDto?> GetByIdAsync(Guid id)
        {
            var objava = await _context.Objave
                .Include(o => o.Autor)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (objava == null) return null;

            return new ObjavaResponseDto
            {
                Id = objava.Id,
                Naslov = objava.Naslov,
                Sadrzaj = objava.Sadrzaj,
                Tip = objava.Tip,
                ImageUrl = objava.ImageUrl,
                AutorId = objava.AutorId,
                AutorIme = objava.Autor.Ime,
                Objavljeno = objava.Objavljeno,
                CreatedAt = objava.CreatedAt,
                UpdatedAt = objava.UpdatedAt
            };
        }

        public async Task<ObjavaResponseDto> CreateAsync(ObjavaCreateDto dto)
        {
            var objava = new Objava
            {
                Id = Guid.NewGuid(),
                Naslov = dto.Naslov,
                Sadrzaj = dto.Sadrzaj,
                Tip = dto.Tip,
                ImageUrl = dto.ImageUrl,
                AutorId = dto.AutorId,
                Objavljeno = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Objave.Add(objava);
            await _context.SaveChangesAsync();

            // Get author info for response
            var autor = await _context.Korisnici.FindAsync(dto.AutorId);

            return new ObjavaResponseDto
            {
                Id = objava.Id,
                Naslov = objava.Naslov,
                Sadrzaj = objava.Sadrzaj,
                Tip = objava.Tip,
                ImageUrl = objava.ImageUrl,
                AutorId = objava.AutorId,
                AutorIme = autor?.Ime,
                Objavljeno = objava.Objavljeno,
                CreatedAt = objava.CreatedAt,
                UpdatedAt = objava.UpdatedAt
            };
        }

        public async Task<ObjavaResponseDto> UpdateAsync(Guid id, ObjavaCreateDto dto)
        {
            var objava = await _context.Objave.FindAsync(id);
            if (objava == null) return null;

            objava.Naslov = dto.Naslov;
            objava.Sadrzaj = dto.Sadrzaj;
            objava.Tip = dto.Tip;
            objava.ImageUrl = dto.ImageUrl;
            objava.AutorId = dto.AutorId;
            objava.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var autor = await _context.Korisnici.FindAsync(dto.AutorId);

            return new ObjavaResponseDto
            {
                Id = objava.Id,
                Naslov = objava.Naslov,
                Sadrzaj = objava.Sadrzaj,
                Tip = objava.Tip,
                ImageUrl = objava.ImageUrl,
                AutorId = objava.AutorId,
                AutorIme = autor?.Ime,
                Objavljeno = objava.Objavljeno,
                CreatedAt = objava.CreatedAt,
                UpdatedAt = objava.UpdatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var objava = await _context.Objave.FindAsync(id);
            if (objava == null) return false;

            _context.Objave.Remove(objava);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ObjavaResponseDto>> GetByAutorAsync(Guid autorId)
        {
            var objave = await _context.Objave
                .Include(o => o.Autor)
                .Where(o => o.AutorId == autorId)
                .ToListAsync();

            return objave.Select(o => new ObjavaResponseDto
            {
                Id = o.Id,
                Naslov = o.Naslov,
                Sadrzaj = o.Sadrzaj,
                Tip = o.Tip,
                ImageUrl = o.ImageUrl,
                AutorId = o.AutorId,
                AutorIme = o.Autor.Ime,
                Objavljeno = o.Objavljeno,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            });
        }

        public async Task<IEnumerable<ObjavaResponseDto>> GetByTipAsync(string tip)
        {
            var objave = await _context.Objave
                .Include(o => o.Autor)
                .Where(o => o.Tip == tip)
                .ToListAsync();

            return objave.Select(o => new ObjavaResponseDto
            {
                Id = o.Id,
                Naslov = o.Naslov,
                Sadrzaj = o.Sadrzaj,
                Tip = o.Tip,
                ImageUrl = o.ImageUrl,
                AutorId = o.AutorId,
                AutorIme = o.Autor.Ime,
                Objavljeno = o.Objavljeno,
                CreatedAt = o.CreatedAt,
                UpdatedAt = o.UpdatedAt
            });
        }
    }
}
