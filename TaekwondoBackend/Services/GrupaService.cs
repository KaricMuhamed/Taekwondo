using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{

        public class GrupaService : IGrupaService
        {
            private readonly ApplicationDbContext _context;

            public GrupaService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<GrupaResponseDto>> GetAllAsync()
            {
                var grupe = await _context.Grupe
                    .Include(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .ToListAsync();

                return grupe.Select(g => MapToResponseDto(g));
            }

            public async Task<GrupaResponseDto?> GetByIdAsync(Guid id)
            {
                var grupa = await _context.Grupe
                    .Include(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (grupa == null) return null;

                return MapToResponseDto(grupa);
            }

            public async Task<GrupaResponseDto> CreateAsync(GrupaCreateDto dto)
            {
                var grupa = new Grupa
                {
                    Id = Guid.NewGuid(),
                    Naziv = dto.Naziv,
                    Opis = dto.Opis,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Grupe.Add(grupa);
                await _context.SaveChangesAsync();

                // Add students to group
                foreach (var ucenikId in dto.UcenikIds)
                {
                    var grupaUcenik = new GrupaUcenik
                    {
                        Id = Guid.NewGuid(),
                        GrupaId = grupa.Id,
                        UcenikId = ucenikId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.GrupaUcenici.Add(grupaUcenik);
                }

                await _context.SaveChangesAsync();

                return await GetByIdAsync(grupa.Id) ?? throw new InvalidOperationException();
            }

                public async Task<GrupaResponseDto> UpdateAsync(Guid id, GrupaCreateDto dto)
            {
                var grupa = await _context.Grupe.FindAsync(id);
                if (grupa == null) return null;

                grupa.Naziv = dto.Naziv;
                grupa.Opis = dto.Opis;
                grupa.UpdatedAt = DateTime.UtcNow;

                // Remove existing student associations
                var existingAssociations = await _context.GrupaUcenici
                    .Where(gu => gu.GrupaId == id)
                    .ToListAsync();
                _context.GrupaUcenici.RemoveRange(existingAssociations);

                // Add new student associations
                foreach (var ucenikId in dto.UcenikIds)
                {
                    var grupaUcenik = new GrupaUcenik
                    {
                        Id = Guid.NewGuid(),
                        GrupaId = grupa.Id,
                        UcenikId = ucenikId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.GrupaUcenici.Add(grupaUcenik);
                }

                await _context.SaveChangesAsync();

                return await GetByIdAsync(grupa.Id) ?? throw new InvalidOperationException();
            }

            public async Task<bool> DeleteAsync(Guid id)
            {
                var grupa = await _context.Grupe.FindAsync(id);
                if (grupa == null) return false;

                _context.Grupe.Remove(grupa);
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<bool> AddUcenikToGrupaAsync(Guid grupaId, Guid ucenikId)
            {
                // Check if association already exists
                var existingAssociation = await _context.GrupaUcenici
                    .FirstOrDefaultAsync(gu => gu.GrupaId == grupaId && gu.UcenikId == ucenikId);

                if (existingAssociation != null) return false;

                var grupaUcenik = new GrupaUcenik
                {
                    Id = Guid.NewGuid(),
                    GrupaId = grupaId,
                    UcenikId = ucenikId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.GrupaUcenici.Add(grupaUcenik);
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<bool> RemoveUcenikFromGrupaAsync(Guid grupaId, Guid ucenikId)
            {
                var association = await _context.GrupaUcenici
                    .FirstOrDefaultAsync(gu => gu.GrupaId == grupaId && gu.UcenikId == ucenikId);

                if (association == null) return false;

                _context.GrupaUcenici.Remove(association);
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<IEnumerable<KorisnikResponseDto>> GetUceniciInGrupaAsync(Guid grupaId)
            {
                var ucenici = await _context.GrupaUcenici
                    .Where(gu => gu.GrupaId == grupaId)
                    .Include(gu => gu.Ucenik)
                    .Select(gu => gu.Ucenik)
                    .ToListAsync();

                return ucenici.Select(u => MapKorisnikToResponseDto(u));
            }

            private static GrupaResponseDto MapToResponseDto(Grupa grupa)
            {
                return new GrupaResponseDto
                {
                    Id = grupa.Id,
                    Naziv = grupa.Naziv,
                    Opis = grupa.Opis,
                    CreatedAt = grupa.CreatedAt,
                    UpdatedAt = grupa.UpdatedAt,
                    Ucenici = grupa.GrupaUcenici?.Select(gu => MapKorisnikToResponseDto(gu.Ucenik)).ToList() ?? new List<KorisnikResponseDto>()
                };
            }

            private static KorisnikResponseDto MapKorisnikToResponseDto(Korisnik korisnik)
            {
                return new KorisnikResponseDto
                {
                    Id = korisnik.Id,
                    Email = korisnik.Email,
                    Ime = korisnik.Ime,
                    Telefon = korisnik.Telefon,
                    Uloga = korisnik.Uloga,
                    Stanje = korisnik.Stanje,
                    Pojas = korisnik.Pojas,
                    DatumPridruzivanja = korisnik.DatumPridruzivanja,
                    Napomene = korisnik.Napomene,
                    CreatedAt = korisnik.CreatedAt,
                    UpdatedAt = korisnik.UpdatedAt
                };
            }
        }
    }
