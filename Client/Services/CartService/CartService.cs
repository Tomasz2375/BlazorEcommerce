using BlazorEcommerce.Client.Services.AuthenticationService;
using BlazorEcommerce.Shared.Dtos;
using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CartService;

public class CartService : ICartService
{
    private readonly ILocalStorageService localStorageService;
    private readonly HttpClient httpClient;
    private readonly IAuthService authService;

    public CartService(
        ILocalStorageService localStorageService,
        HttpClient httpClient,
        IAuthService authService)
    {
        this.localStorageService = localStorageService;
        this.httpClient = httpClient;
        this.authService = authService;
    }

    public event Action OnChange;

    public async Task AddToCart(CartItem cartItem)
    {
        if (await authService.IsUserAuthenticated())
        {
            await httpClient.PostAsJsonAsync("api/cart/add", cartItem);
        }
        else
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
        }

        await GetCartItemsCount();
    }

    public async Task<List<CartProductResponseDto>> GetCartProducts()
    {
        
        if (await authService.IsUserAuthenticated())
        {
            var response = await httpClient.GetFromJsonAsync<ServiceResponse<List<CartProductResponseDto>>>("api/cart");
            return response!.Data;
        }
        else
        {
            var cartItems = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            if (cartItems is null)
            {
                cartItems = new List<CartItem>();
            }
            var response = await httpClient.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDto>>>();

            return cartProducts!.Data!;
        }

    }

    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        if (await authService.IsUserAuthenticated())
        {
            await httpClient.DeleteAsync($"api/cart/{productId}/{productTypeId}");
        }
        else
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");

            if (cart is null)
            {
                return;
            }

            var cartItem = cart.Find(
                x => x.ProductId == productId &&
                x.ProductTypeId == productTypeId);

            if (cartItem is null)
            {
                return;
            }

            cart.Remove(cartItem);
            await localStorageService.SetItemAsync("cart", cart);
        }
    }

    public async Task StoreCartItems(bool emptyLocalCart)
    {
        var localCart = await localStorageService.GetItemAsync<List<CartItem>>("cart");

        if (localCart is null)
        {
            return;
        }

        await httpClient.PostAsJsonAsync("api/cart", localCart);

        if (emptyLocalCart)
        {
            await localStorageService.RemoveItemAsync("cart");
        }
    }

    public async Task UpdateQuantity(CartProductResponseDto product)
    {
        if (await authService.IsUserAuthenticated())
        {
            var request = new CartItem()
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity
            };

            await httpClient.PutAsJsonAsync("api/cart/update-quantity", request);
        }
        else
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

    public async Task GetCartItemsCount()
    {
        if (await authService.IsUserAuthenticated())
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
            var count = result!.Data;

            await localStorageService.SetItemAsync<int>("cartItemsCount", count);
        }
        else
        {
            var cart = await localStorageService.GetItemAsync<List<CartItem>>("cart");
            await localStorageService.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
        }

        OnChange.Invoke();
    }
}
