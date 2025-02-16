using System.Net.Http.Json;
using TakwondoFrontend.Models;

namespace TakwondoFrontend.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(User user)
        {
            var loginRequest = new
            {
                username = user.Username,
                password = user.Password
            };

            var response = await _httpClient.PostAsJsonAsync("/api/Auth/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var tokens = await response.Content.ReadFromJsonAsync<Tokens>();
                // Store the tokens (e.g., in localStorage or sessionStorage)

                return true;
            }

            return false;
        }
    }
}
