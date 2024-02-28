using BlazorEcommerce.Shared.Dtos;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CartService;

public class CartService : ICartService
{
    private readonly ILocalStorageService localStorageService;
    private readonly HttpClient httpClient;

    public CartService(ILocalStorageService localStorageService, HttpClient httpClient)
    {
        this.localStorageService = localStorageService;
        this.httpClient = httpClient;
    }

    public event Action OnChange;

    public async Task AddToCart(CartItem cartItem)
    {
        var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");

        if (cart is null)
        {
            cart = new List<CartItem>();
        }

        cart.Add(cartItem);

        await localStorageService.SetItemAsync("cart", cart);
        OnChange.Invoke();
    }

    public async Task<List<CartItem>> GetCartItems()
    {
        var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");

        if (cart is null)
        {
            cart = new List<CartItem>();
        }

        return cart;
    }

    public async Task<List<CartProductResponseDto>> GetCartProducts()
    {
        var cartItems = await localStorageService.GetItemAsync<List<CartItem>>("cart");

        var response = await httpClient.PostAsJsonAsync("api/cart/products", cartItems);
        var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDto>>>();

        return cartProducts!.Data!;
    }
}
