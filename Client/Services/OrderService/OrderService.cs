﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

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