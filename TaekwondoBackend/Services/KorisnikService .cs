using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;
using TaekwondoBackend.Services;

namespace TaekwondoBackend.Services
{
    public class KorisnikService : IKorisnikService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        public KorisnikService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<IEnumerable<KorisnikResponseDto>> GetAllAsync()
        {
            var korisnici = await _context.Korisnici.ToListAsync();

            return korisnici.Select(k => MapToResponseDto(k));
        }

        public async Task<KorisnikResponseDto?> GetByIdAsync(Guid id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik == null)
                return null;

            return MapToResponseDto(korisnik);
        }

        public async Task<KorisnikResponseDto> CreateAsync(KorisnikCreateDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var korisnik = new Korisnik
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Ime = dto.Ime,
                Telefon = dto.Telefon,
                Uloga = dto.Uloga,
                Stanje = dto.Stanje,
                Pojas = dto.Pojas,
                DatumPridruzivanja = DateTime.UtcNow.Date,
                Napomene = dto.Napomene,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Korisnici.Add(korisnik);
            await _context.SaveChangesAsync();

            return MapToResponseDto(korisnik);
        }

        public async Task<KorisnikResponseDto> UpdateAsync(Guid id, KorisnikCreateDto dto)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik == null)
                return null;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                korisnik.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            korisnik.Email = dto.Email;
            korisnik.Ime = dto.Ime;
            korisnik.Telefon = dto.Telefon;
            korisnik.Uloga = dto.Uloga;
            korisnik.Stanje = dto.Stanje;
            korisnik.Pojas = dto.Pojas;
            korisnik.Napomene = dto.Napomene;
            korisnik.UpdatedAt = DateTime.UtcNow;

            _context.Entry(korisnik).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return MapToResponseDto(korisnik);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);
            if (korisnik == null)
                return false;

            _context.Korisnici.Remove(korisnik);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<KorisnikResponseDto>> GetByUlogaAsync(string uloga)
        {
            var korisnici = await _context.Korisnici
                .Where(k => k.Uloga == uloga)
                .ToListAsync();

            return korisnici.Select(k => MapToResponseDto(k));
        }

        public async Task<IEnumerable<KorisnikResponseDto>> GetByPojasAsync(string pojas)
        {
            var korisnici = await _context.Korisnici
                .Where(k => k.Pojas == pojas)
                .ToListAsync();

            return korisnici.Select(k => MapToResponseDto(k));
        }

        public async Task<KorisnikResponseDto?> GetByEmailAsync(string email)
        {
            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Email == email);

            if (korisnik == null)
                return null;

            return MapToResponseDto(korisnik);
        }

        public async Task<IEnumerable<KorisnikResponseDto>> GetActiveAsync()
        {
            var korisnici = await _context.Korisnici
                .Where(k => k.Stanje == "aktivan")
                .ToListAsync();

            return korisnici.Select(k => MapToResponseDto(k));
        }

        public async Task<IEnumerable<KorisnikResponseDto>> GetTreneriAsync()
        {
            var korisnici = await _context.Korisnici
                .Where(k => k.Uloga == "trener")
                .ToListAsync();

            return korisnici.Select(k => MapToResponseDto(k));
        }

        public async Task<IEnumerable<KorisnikResponseDto>> GetUceniciAsync()
        {
            var korisnici = await _context.Korisnici
                .Where(k => k.Uloga == "ucenik")
                .ToListAsync();

            return korisnici.Select(k => MapToResponseDto(k));
        }

        public async Task<IEnumerable<KorisnikResponseDto>> GetByMinimumBeltAsync(string minimumPojas)
        {
            // Belt hierarchy for filtering
            var beltHierarchy = new Dictionary<string, int>
            {
                { "bijeli", 1 },
                { "zuti", 2 },
                { "narancasti", 3 },
                { "zeleni", 4 },
                { "plavi", 5 },
                { "smedi", 6 },
                { "crni_1_dan", 7 },
                { "crni_2_dan", 8 },
                { "crni_3_dan", 9 },
                { "crni_4_dan", 10 },
                { "crni_5_dan", 11 },
                { "crni_6_dan", 12 },
                { "crni_7_dan", 13 },
                { "crni_8_dan", 14 },
                { "crni_9_dan", 15 },
                { "crni_10_dan", 16 }
            };

            if (!beltHierarchy.ContainsKey(minimumPojas))
                return new List<KorisnikResponseDto>();

            var minimumLevel = beltHierarchy[minimumPojas];

            var korisnici = await _context.Korisnici.ToListAsync();

            var filteredKorisnici = korisnici
                .Where(k => beltHierarchy.ContainsKey(k.Pojas) &&
                            beltHierarchy[k.Pojas] >= minimumLevel)
                .ToList();

            return filteredKorisnici.Select(k => MapToResponseDto(k));
        }

        // Helper method to map entity to DTO
        private static KorisnikResponseDto MapToResponseDto(Korisnik korisnik)
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

        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            var korisnik = await _context.Korisnici
                .FirstOrDefaultAsync(k => k.Email == email);

            if (korisnik == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, korisnik.PasswordHash);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email
                var korisnik = await _context.Korisnici
                    .FirstOrDefaultAsync(k => k.Email == loginDto.Email);

                if (korisnik == null)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Nevalidni podaci za prijavu.",
                        Korisnik = null
                    };
                }

                // Verify password
                var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, korisnik.PasswordHash);

                if (!isPasswordValid)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Nevalidni podaci za prijavu.",
                        Korisnik = null
                    };
                }

                // Check if user is active
                if (korisnik.Stanje != "aktivan")
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Racun nije aktivan. Kontaktirajte administratora.",
                        Korisnik = null
                    };
                }

                // Successful login

                var result = new LoginResponseDto
                {
                    Success = true,
                    Message = "Uspjesna prijava.",
                    Korisnik = MapToResponseDto(korisnik),
                };

                if (result.Success)
                {
                    var token = _jwtService.GenerateToken(result.Korisnik);
                    result.Token = token;
                    result.TokenExpiration = DateTime.UtcNow.AddHours(24);
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception in production
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Greska prilikom prijave.",
                    Korisnik = null
                };
            }

            
        }
    }
}
