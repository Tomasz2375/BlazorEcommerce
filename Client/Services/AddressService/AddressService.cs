using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly HttpClient httpClient;

    public AddressService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Address> AddOrUpdateAddress(Address address)
    {
        var response = await httpClient.PostAsJsonAsync("api/address", address);

        return response.Content
            .ReadFromJsonAsync<ServiceResponse<Address>>()
            .Result!
            .Data!;
    }

    public async Task<Address> GetAddress()
    {
        var response = await httpClient
            .GetFromJsonAsync<ServiceResponse<Address>>("api/address");
        return response!.Data!;
    }
}
