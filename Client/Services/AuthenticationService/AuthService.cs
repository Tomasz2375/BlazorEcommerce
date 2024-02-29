using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.AuthenticationService;

public class AuthService : IAuthService
{
    private readonly HttpClient httpClient;

    public AuthService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ServiceResponse<string>> Login(UserLogin request)
    {
        var result = await httpClient.PostAsJsonAsync("api/authentication/login", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
    }

    public async Task<ServiceResponse<int>> Register(UserRegister request)
    {
        var result = await httpClient.PostAsJsonAsync("api/authentication/register", request);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
    }
}
