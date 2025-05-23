using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{
    public interface IGrupaService
    {
        Task<IEnumerable<GrupaResponseDto>> GetAllAsync();
        Task<GrupaResponseDto?> GetByIdAsync(Guid id);
        Task<GrupaResponseDto> CreateAsync(GrupaCreateDto dto);
        Task<GrupaResponseDto> UpdateAsync(Guid id, GrupaCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> AddUcenikToGrupaAsync(Guid grupaId, Guid ucenikId);
        Task<bool> RemoveUcenikFromGrupaAsync(Guid grupaId, Guid ucenikId);
        Task<IEnumerable<KorisnikResponseDto>> GetUceniciInGrupaAsync(Guid grupaId);
    }
}
