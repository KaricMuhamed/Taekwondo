using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaekwondoBackend.Data;
using TaekwondoBackend.Entities;

namespace TaekwondoBackend.Repositories.Member
{
    public class MemberRepository : IMemberRepository
    {
        private readonly TaekwondoDbContext _context;

        public MemberRepository(TaekwondoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Members>> GetAllAsync()
        {
            return await _context.Members
                .Include(m => m.UserMembers)
                .ToListAsync();
        }

        public async Task<Members?> GetByIdAsync(int id)
        {
            return await _context.Members
                .Include(m => m.UserMembers)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Members member) =>
            await _context.Members.AddAsync(member);

        public void Update(Members member) =>
            _context.Members.Update(member);

        public void Delete(Members member) =>
            _context.Members.Remove(member);

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}