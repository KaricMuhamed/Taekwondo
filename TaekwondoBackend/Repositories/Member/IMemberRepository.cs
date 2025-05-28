using System.Collections.Generic;
using System.Threading.Tasks;
using TaekwondoBackend.Entities;

namespace TaekwondoBackend.Repositories.Member
{
    public interface IMemberRepository
    {
        Task<List<Members>> GetAllAsync();
        Task<Members?> GetByIdAsync(int id);
        Task AddAsync(Members member);
        void Update(Members member);
        void Delete(Members member);
        Task SaveChangesAsync();
    }
}