using System.Collections.Generic;
using System.Threading.Tasks;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services.Member
{
    public interface IMemberService
    {
        Task<List<MembersDto>> GetAllAsync();
        Task<MembersDto?> GetByIdAsync(int id);
        Task<MembersDto> CreateAsync(MembersDto dto);
        Task<bool> UpdateAsync(int id, MembersDto dto);
        Task<bool> DeleteAsync(int id);
    }
}