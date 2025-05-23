using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{

        public class TreningService : ITreningService
        {
            private readonly ApplicationDbContext _context;

            public TreningService(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<TreningResponseDto>> GetAllAsync()
            {
                var treninzi = await _context.Treninzi
                    .Include(t => t.TreningTreneri)
                    .ThenInclude(tt => tt.Trener)
                    .Include(t => t.TreningGrupe)
                    .ThenInclude(tg => tg.Grupa)
                    .ThenInclude(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .OrderBy(t => t.Datum)
                    .ThenBy(t => t.VrijemeOd)
                    .ToListAsync();

                return treninzi.Select(t => MapToResponseDto(t));
            }

            public async Task<TreningResponseDto?> GetByIdAsync(Guid id)
            {
                var trening = await _context.Treninzi
                    .Include(t => t.TreningTreneri)
                    .ThenInclude(tt => tt.Trener)
                    .Include(t => t.TreningGrupe)
                    .ThenInclude(tg => tg.Grupa)
                    .ThenInclude(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (trening == null) return null;

                return MapToResponseDto(trening);
            }

            public async Task<TreningResponseDto> CreateAsync(TreningCreateDto dto)
            {
                var trening = new Trening
                {
                    Id = Guid.NewGuid(),
                    Naziv = dto.Naziv,
                    Opis = dto.Opis,
                    Datum = dto.Datum.Date,
                    VrijemeOd = dto.VrijemeOd,
                    VrijemeDo = dto.VrijemeDo,
                    Lokacija = dto.Lokacija,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Treninzi.Add(trening);
                await _context.SaveChangesAsync();

                // Add trainers
                foreach (var trenerId in dto.TrenerIds)
                {
                    var treningTrener = new TreningTrener
                    {
                        Id = Guid.NewGuid(),
                        TreningId = trening.Id,
                        TrenerId = trenerId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.TreningTreneri.Add(treningTrener);
                }

                // Add groups
                foreach (var grupaId in dto.GrupaIds)
                {
                    var treningGrupa = new TreningGrupa
                    {
                        Id = Guid.NewGuid(),
                        TreningId = trening.Id,
                        GrupaId = grupaId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.TreningGrupe.Add(treningGrupa);
                }

                await _context.SaveChangesAsync();

                return await GetByIdAsync(trening.Id) ?? throw new InvalidOperationException();
            }

            public async Task<TreningResponseDto> UpdateAsync(Guid id, TreningCreateDto dto)
            {
                var trening = await _context.Treninzi.FindAsync(id);
                if (trening == null) return null;

                trening.Naziv = dto.Naziv;
                trening.Opis = dto.Opis;
                trening.Datum = dto.Datum.Date;
                trening.VrijemeOd = dto.VrijemeOd;
                trening.VrijemeDo = dto.VrijemeDo;
                trening.Lokacija = dto.Lokacija;
                trening.UpdatedAt = DateTime.UtcNow;

                // Remove existing associations
                var existingTreneri = await _context.TreningTreneri
                    .Where(tt => tt.TreningId == id)
                    .ToListAsync();
                _context.TreningTreneri.RemoveRange(existingTreneri);

                var existingGrupe = await _context.TreningGrupe
                    .Where(tg => tg.TreningId == id)
                    .ToListAsync();
                _context.TreningGrupe.RemoveRange(existingGrupe);

                // Add new associations
                foreach (var trenerId in dto.TrenerIds)
                {
                    var treningTrener = new TreningTrener
                    {
                        Id = Guid.NewGuid(),
                        TreningId = trening.Id,
                        TrenerId = trenerId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.TreningTreneri.Add(treningTrener);
                }

                foreach (var grupaId in dto.GrupaIds)
                {
                    var treningGrupa = new TreningGrupa
                    {
                        Id = Guid.NewGuid(),
                        TreningId = trening.Id,
                        GrupaId = grupaId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.TreningGrupe.Add(treningGrupa);
                }

                await _context.SaveChangesAsync();

                return await GetByIdAsync(trening.Id) ?? throw new InvalidOperationException();
            }

            public async Task<bool> DeleteAsync(Guid id)
            {
                var trening = await _context.Treninzi.FindAsync(id);
                if (trening == null) return false;

                _context.Treninzi.Remove(trening);
                await _context.SaveChangesAsync();
                return true;
            }

            public async Task<IEnumerable<TreningResponseDto>> GetTreninziByDateAsync(DateTime datum)
            {
                var treninzi = await _context.Treninzi
                    .Include(t => t.TreningTreneri)
                    .ThenInclude(tt => tt.Trener)
                    .Include(t => t.TreningGrupe)
                    .ThenInclude(tg => tg.Grupa)
                    .ThenInclude(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .Where(t => t.Datum.Date == datum.Date)
                    .OrderBy(t => t.VrijemeOd)
                    .ToListAsync();

                return treninzi.Select(t => MapToResponseDto(t));
            }

            public async Task<IEnumerable<TreningResponseDto>> GetTreninziByTrenerAsync(Guid trenerId)
            {
                var treninzi = await _context.Treninzi
                    .Include(t => t.TreningTreneri)
                    .ThenInclude(tt => tt.Trener)
                    .Include(t => t.TreningGrupe)
                    .ThenInclude(tg => tg.Grupa)
                    .ThenInclude(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .Where(t => t.TreningTreneri.Any(tt => tt.TrenerId == trenerId))
                    .OrderBy(t => t.Datum)
                    .ThenBy(t => t.VrijemeOd)
                    .ToListAsync();

                return treninzi.Select(t => MapToResponseDto(t));
            }

            public async Task<IEnumerable<TreningResponseDto>> GetTreninziByGrupaAsync(Guid grupaId)
            {
                var treninzi = await _context.Treninzi
                    .Include(t => t.TreningTreneri)
                    .ThenInclude(tt => tt.Trener)
                    .Include(t => t.TreningGrupe)
                    .ThenInclude(tg => tg.Grupa)
                    .ThenInclude(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .Where(t => t.TreningGrupe.Any(tg => tg.GrupaId == grupaId))
                    .OrderBy(t => t.Datum)
                    .ThenBy(t => t.VrijemeOd)
                    .ToListAsync();

                return treninzi.Select(t => MapToResponseDto(t));
            }

            public async Task<IEnumerable<TreningResponseDto>> GetUpcomingTreninziAsync()
            {
                var today = DateTime.UtcNow.Date;
                var treninzi = await _context.Treninzi
                    .Include(t => t.TreningTreneri)
                    .ThenInclude(tt => tt.Trener)
                    .Include(t => t.TreningGrupe)
                    .ThenInclude(tg => tg.Grupa)
                    .ThenInclude(g => g.GrupaUcenici)
                    .ThenInclude(gu => gu.Ucenik)
                    .Where(t => t.Datum >= today)
                    .OrderBy(t => t.Datum)
                    .ThenBy(t => t.VrijemeOd)
                    .ToListAsync();

                return treninzi.Select(t => MapToResponseDto(t));
            }

            private static TreningResponseDto MapToResponseDto(Trening trening)
            {
                return new TreningResponseDto
                {
                    Id = trening.Id,
                    Naziv = trening.Naziv,
                    Opis = trening.Opis,
                    Datum = trening.Datum,
                    VrijemeOd = trening.VrijemeOd,
                    VrijemeDo = trening.VrijemeDo,
                    Lokacija = trening.Lokacija,
                    CreatedAt = trening.CreatedAt,
                    UpdatedAt = trening.UpdatedAt,
                    Treneri = trening.TreningTreneri?.Select(tt => MapKorisnikToResponseDto(tt.Trener)).ToList() ?? new List<KorisnikResponseDto>(),
                    Grupe = trening.TreningGrupe?.Select(tg => MapGrupaToResponseDto(tg.Grupa)).ToList() ?? new List<GrupaResponseDto>()
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

            private static GrupaResponseDto MapGrupaToResponseDto(Grupa grupa)
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
        }
    }
