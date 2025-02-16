using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json.Nodes;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private readonly ILocalStorageService localStorageService;

    public CustomAuthStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        this.httpClient = httpClient;
        this.localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorageService.GetItemAsync<string>("accessToken");

        var identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                // Set the Authorization header with the token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                identity = GetClaimsFromToken(token);  // Get claims from the token
            }
            catch (Exception)
            {
                // Handle error (invalid token)
            }
        }

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task<FormResult> LoginAsync(string email, string password)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", new { email, password });

            if (response.IsSuccessStatusCode)
            {
                var strResponse = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonNode.Parse(strResponse);
                var accessToken = jsonResponse?["accessToken"]?.ToString();
                var refreshToken = jsonResponse?["refreshToken"]?.ToString();

                if (accessToken != null && refreshToken != null)
                {
                    // Store the tokens
                    await localStorageService.SetItemAsync("accessToken", accessToken);
                    await localStorageService.SetItemAsync("refreshToken", refreshToken);

                    // Set the Authorization header with the new token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                    return new FormResult { Succeded = true };
                }
            }

            return new FormResult { Succeded = false, Errors = new[] { "Bad email or password" } };
        }
        catch (Exception)
        {
            return new FormResult { Succeded = false, Errors = new[] { "Connection error" } };
        }
    }

    // Helper method to extract claims from the token
    private ClaimsIdentity GetClaimsFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
        var claims = jwtToken?.Claims.Select(c => new Claim(c.Type, c.Value)).ToList() ?? new List<Claim>();
        return new ClaimsIdentity(claims, "Bearer");
    }

    public async Task LogoutAsync()
    {
        await localStorageService.RemoveItemAsync("accessToken");
        await localStorageService.RemoveItemAsync("refreshToken");

        httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

public class FormResult
{
    public bool Succeded { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();
}
