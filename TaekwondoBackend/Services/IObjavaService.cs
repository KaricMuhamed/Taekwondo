using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{
    public interface IObjavaService
    {
        Task<IEnumerable<ObjavaResponseDto>> GetAllAsync();
        Task<ObjavaResponseDto?> GetByIdAsync(Guid id);
        Task<ObjavaResponseDto> CreateAsync(ObjavaCreateDto dto);
        Task<ObjavaResponseDto> UpdateAsync(Guid id, ObjavaCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<ObjavaResponseDto>> GetByAutorAsync(Guid autorId);
        Task<IEnumerable<ObjavaResponseDto>> GetByTipAsync(string tip);
    }
}
