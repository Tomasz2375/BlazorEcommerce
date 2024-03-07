using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.AuthenticationService;

public class AuthService : IAuthService
{
    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider stateProvider;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider stateProvider)
    {
        this.httpClient = httpClient;
        this.stateProvider = stateProvider;
    }

    public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
    {
        var result = await httpClient.PostAsJsonAsync("api/authentication/change-password", request.Password);

        return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
    }

    public async Task<bool> IsUserAuthenticated()
    {
        return (await stateProvider.GetAuthenticationStateAsync())
            .User
            .Identity!
            .IsAuthenticated;
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
