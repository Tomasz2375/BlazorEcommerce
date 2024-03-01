﻿using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.CartService;

public class CartService : ICartService
{
    private readonly DataContext dataContext;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CartService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        this.dataContext = dataContext;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<int>> GetCartItemsCount()
    {
        var count = (await dataContext.CartItems!
            .Where(x => x.UserId == getUserId())
            .ToListAsync()).Count;

        return new ServiceResponse<int>()
        {
            Data = count,
            Sucess = true,
        };
    }

    public async Task<ServiceResponse<List<CartProductResponseDto>>> GetCartPrductAsync(List<CartItem> cartItems)
    {
        var result = new ServiceResponse<List<CartProductResponseDto>>()
        {
            Data = new List<CartProductResponseDto>(),
        };

        foreach (var cartItem in cartItems)
        {
            var product = await dataContext.Products!
                .Where(x => x.Id == cartItem.ProductId)
                .FirstOrDefaultAsync();

            if (product is null)
            {
                continue;
            }

            var productVariant = await dataContext.ProductVariants!
                .Include(x => x.ProductType)
                .Where(x => x.ProductId == cartItem.ProductId &&
                    x.ProductTypeId == cartItem.ProductTypeId)
                .FirstOrDefaultAsync();

            if (productVariant is null)
            {
                continue;
            }

            var cartProduct = new CartProductResponseDto()
            {
                ProductId = product.Id,
                Title = product.Title,
                ImageUrl = product.ImageUrl,
                Price = productVariant.Price,
                ProductType = productVariant.ProductType.Name,
                ProductTypeId = productVariant.ProductTypeId,
                Quantity = cartItem.Quantity,
            };

            result.Data.Add(cartProduct);
        }

        return result;
    }

    public async Task<ServiceResponse<List<CartProductResponseDto>>> GetDbCartProducts()
    {
        return await GetCartPrductAsync(await dataContext.CartItems
            .Where(x => x.UserId == getUserId()).ToListAsync());
    }

    public async Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItems(List<CartItem> cartItems)
    {
        cartItems.ForEach(cartItem => cartItem.UserId = getUserId());
        dataContext.CartItems!.AddRange(cartItems);
        await dataContext.SaveChangesAsync();

        return await GetDbCartProducts();
    }
    private int getUserId() =>
        int.Parse(httpContextAccessor.HttpContext!
            .User.FindFirstValue(ClaimTypes.NameIdentifier));
}
