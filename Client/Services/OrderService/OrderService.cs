using BlazorEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly HttpClient httpClient;
    private readonly AuthenticationStateProvider authenticationStateProvider;
    private readonly NavigationManager navigationManager;

    public OrderService(
        HttpClient httpClient,
        AuthenticationStateProvider authenticationStateProvider,
        NavigationManager navigationManager)
    {
        this.httpClient = httpClient;
        this.authenticationStateProvider = authenticationStateProvider;
        this.navigationManager = navigationManager;
    }

    public async Task<OrderDetailsResponse> GetOrderDetails(int orderId)
    {
        var result = await httpClient.GetFromJsonAsync<ServiceResponse<OrderDetailsResponse>>($"api/order/{orderId}");

        return result!.Data!;
    }

    public async Task<List<OrderOverviewResponse>> GetOrders()
    {
        var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponse>>>("api/order");
        return result!.Data!;
    }

    public async Task PlaceOrder()
    {
        if (await isUserAuthenticated())
        {
            await httpClient.PostAsync("api/order", null);
        }
        else
        {
            navigationManager.NavigateTo("login");
        }
    }

    #region Private methods
    private async Task<bool> isUserAuthenticated() =>
    (await authenticationStateProvider.GetAuthenticationStateAsync())
        .User.Identity!.IsAuthenticated;
    #endregion
}
