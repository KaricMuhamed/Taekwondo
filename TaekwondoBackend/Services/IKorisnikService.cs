using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{
    public interface IKorisnikService
    {
        Task<IEnumerable<KorisnikResponseDto>> GetAllAsync();
        Task<KorisnikResponseDto?> GetByIdAsync(Guid id);
        Task<KorisnikResponseDto> CreateAsync(KorisnikCreateDto dto);
        Task<KorisnikResponseDto> UpdateAsync(Guid id, KorisnikCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<KorisnikResponseDto>> GetByUlogaAsync(string uloga);
        Task<IEnumerable<KorisnikResponseDto>> GetByPojasAsync(string pojas);
        Task<KorisnikResponseDto?> GetByEmailAsync(string email);
        Task<IEnumerable<KorisnikResponseDto>> GetActiveAsync();
        Task<IEnumerable<KorisnikResponseDto>> GetTreneriAsync();
        Task<IEnumerable<KorisnikResponseDto>> GetUceniciAsync();
        Task<IEnumerable<KorisnikResponseDto>> GetByMinimumBeltAsync(string minimumPojas);
        Task<bool> VerifyPasswordAsync(string email, string password);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);


    }
}

