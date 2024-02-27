using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.CartService;

public interface ICartSerwice
{
    Task<ServiceResponse<List<CartProductResponseDto>>> GetCartPrductAsync(List<CartItem> cartItems);
}
