using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{
    public interface ITreningService
    {
        Task<IEnumerable<TreningResponseDto>> GetAllAsync();
        Task<TreningResponseDto?> GetByIdAsync(Guid id);
        Task<TreningResponseDto> CreateAsync(TreningCreateDto dto);
        Task<TreningResponseDto> UpdateAsync(Guid id, TreningCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<TreningResponseDto>> GetTreninziByDateAsync(DateTime datum);
        Task<IEnumerable<TreningResponseDto>> GetTreninziByTrenerAsync(Guid trenerId);
        Task<IEnumerable<TreningResponseDto>> GetTreninziByGrupaAsync(Guid grupaId);
        Task<IEnumerable<TreningResponseDto>> GetUpcomingTreninziAsync();
    }
}
