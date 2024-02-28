using BlazorEcommerce.Shared;
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

        var sameItem = cart.FirstOrDefault(x => x.ProductId == cartItem.ProductId &&
            x.ProductTypeId == cartItem.ProductTypeId);

        if (sameItem is null)
        {
            cart.Add(cartItem);
        }
        else
        {
            sameItem.Quantity += cartItem.Quantity;
        }

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

    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");

        if (cart is null)
        {
            return;
        }

        var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);

        if (cartItem is null)
        {
            return;
        }
        cart.Remove(cartItem);
        await localStorageService.SetItemAsync("cart", cart);
        OnChange.Invoke();
    }

    public async Task UpdateQuantity(CartProductResponseDto product)
    {
        var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");

        if (cart is null)
        {
            return;
        }
        var cartItem = cart.Find(x => x.ProductId == product.ProductId &&
            x.ProductTypeId == product.ProductTypeId);

        if (cartItem is null)
        {
            return;
        }
        cartItem.Quantity = product.Quantity;
        await localStorageService.SetItemAsync("cart", cart);
    }
}
