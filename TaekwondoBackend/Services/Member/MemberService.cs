using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaekwondoBackend.Entities;
using TaekwondoBackend.Models;
using TaekwondoBackend.Repositories.Member;

namespace TaekwondoBackend.Services.Member
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly Data.TaekwondoDbContext context;

        public MemberService(IMemberRepository repository, IConfiguration configuration, Data.TaekwondoDbContext context)
        {
            _repository = repository;
            _configuration = configuration;
            this.context = context; 
        }

        public async Task<List<MembersDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var dtos = new List<MembersDto>();  
            foreach (var entity in entities)
            {
                dtos.Add(new MembersDto {
                    Id = entity.Id,
                    Name = entity.Name,
                    Surname = entity.Surname,
                    BirthDate = entity.BirthDate,
                    ContactNumber = entity.ContactNumber,
                    Adresse = entity.Adresse,
                    CurrentBeltId = entity.CurrentBeltId,
                    UserIds = entity.UserMembers?.Select(um => um.UserId).ToList() ?? new List<Guid>()
                });
            }
            return dtos;
        }

        public async Task<MembersDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return new MembersDto {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                BirthDate = entity.BirthDate,
                ContactNumber = entity.ContactNumber,
                Adresse = entity.Adresse,
                CurrentBeltId = entity.CurrentBeltId,
                UserIds = entity.UserMembers.Select(um => um.UserId).ToList()

            };
        }

        public async Task<MembersDto> CreateAsync(MembersDto dto)
        {
            var entity = new Members
            {
                Name = dto.Name,
                Surname = dto.Surname,
                BirthDate = dto.BirthDate,
                ContactNumber = dto.ContactNumber,
                Adresse = dto.Adresse,
                CurrentBeltId = dto.CurrentBeltId,
                DateOfJoining = DateTime.Now,
                UserMembers = new List<UserMember>() 
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            if (dto.UserIds != null && dto.UserIds.Any())
            {
                var userMembers = new List<UserMember>();
                foreach (var userId in dto.UserIds)
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                    if (user != null)
                    {
                        entity.UserMembers.Add(new UserMember
                        {
                            UserId = userId,
                            MemberId = entity.Id,
                            User = user,
                            Member = entity
                        });
                    }
                }

                await _repository.SaveChangesAsync();
            }

            return new MembersDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname,
                BirthDate = entity.BirthDate,
                ContactNumber = entity.ContactNumber,
                Adresse = entity.Adresse,
                CurrentBeltId = entity.CurrentBeltId,
                UserIds = entity.UserMembers.Select(um => um.UserId).ToList()
            };
        }

        public async Task<bool> UpdateAsync(int id, MembersDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.Name = dto.Name;
            existing.Surname = dto.Surname;
            existing.BirthDate = dto.BirthDate;
            existing.ContactNumber = dto.ContactNumber;
            existing.Adresse = dto.Adresse;
            existing.CurrentBeltId = dto.CurrentBeltId;

            var existingRelationships = await context.UserMembers
               .Where(um => um.MemberId == id)
               .ToListAsync();

            context.UserMembers.RemoveRange(existingRelationships);

            existing.UserMembers.Clear();

            if (dto.UserIds != null)
            {
                foreach (var userId in dto.UserIds)
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    if (user == null) continue;
                    context.UserMembers.Add(new UserMember
                    {
                        UserId = userId,
                        MemberId = existing.Id,
                        User = user,
                        Member = existing
                    });
                }
            }

            _repository.Update(existing);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            _repository.Delete(existing);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
