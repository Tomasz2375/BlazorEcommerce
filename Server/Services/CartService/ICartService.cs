using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.CartService;

public interface ICartService
{
    Task<ServiceResponse<List<CartProductResponseDto>>> GetCartPrductAsync(List<CartItem> cartItems);
    Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItems(List<CartItem> cartItems);
    Task<ServiceResponse<int>> GetCartItemsCount();
    Task<ServiceResponse<List<CartProductResponseDto>>> GetDbCartProducts();
}
