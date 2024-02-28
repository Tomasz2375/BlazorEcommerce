﻿using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Client.Services.CartService;

public interface ICartService
{
    event Action OnChange;
    Task AddToCart(CartItem cartItem);
    Task<List<CartItem>> GetCartItems();
    Task<List<CartProductResponseDto>> GetCartProducts();
}
