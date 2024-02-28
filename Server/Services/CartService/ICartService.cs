﻿using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.CartService;

public interface ICartService
{
    Task<ServiceResponse<List<CartProductResponseDto>>> GetCartPrductAsync(List<CartItem> cartItems);
}