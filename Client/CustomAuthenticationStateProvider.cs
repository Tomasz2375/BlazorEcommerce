using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorEcommerce.Client;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorageService;
    private readonly HttpClient httpClient;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
    {
        this.localStorageService = localStorageService;
        this.httpClient = httpClient;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string authenticationToken = await localStorageService.GetItemAsStringAsync("authToken");

        var identity = new ClaimsIdentity();
        httpClient.DefaultRequestHeaders.Authorization = null;

        if (!string.IsNullOrEmpty(authenticationToken))
        {
            try
            {
                identity = new ClaimsIdentity(parseClaimsFromJwt(authenticationToken), "jwt");
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        authenticationToken.Replace("\"", ""));
            }
            catch
            {
                await localStorageService.RemoveItemAsync("authToken");
                identity = new ClaimsIdentity();
            }
        }

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    private byte[] parseBase64WithoutPadding(string base64)
    {
        switch(base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private IEnumerable<Claim> parseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = parseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer
            .Deserialize<Dictionary<string, object>>(jsonBytes);

        var claims = keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString()));
        return claims;
    }
}
