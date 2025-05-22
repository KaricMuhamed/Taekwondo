using System.Security.Claims;
using TaekwondoBackend.Models;

namespace TaekwondoBackend.Services
{
    public interface IJwtService
    {
        string GenerateToken(KorisnikResponseDto korisnik);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
