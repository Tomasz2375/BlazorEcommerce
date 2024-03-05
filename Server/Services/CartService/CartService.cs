using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.AuthenticationServices;
using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.CartService;

public class CartService : ICartService
{
    private readonly DataContext dataContext;
    private readonly IAuthenticationService authenticationService;

    public CartService(DataContext dataContext, IAuthenticationService authenticationService)
    {
        this.dataContext = dataContext;
        this.authenticationService = authenticationService;
    }

    public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
    {
        cartItem.UserId = authenticationService.GetUserId();

        var sameItem = await dataContext.CartItems!
            .FirstOrDefaultAsync(x =>
                x.ProductId == cartItem.ProductId &&
                x.ProductTypeId == cartItem.ProductTypeId &&
                x.UserId == authenticationService.GetUserId());

        if (sameItem is null)
        {
            dataContext.CartItems!.Add(cartItem);
        }
        else
        {
            sameItem.Quantity += cartItem.Quantity;
        }

        await dataContext.SaveChangesAsync();
        return new ServiceResponse<bool> { Data = true, Sucess = true };

    }

    public async Task<ServiceResponse<int>> GetCartItemsCount()
    {
        var count = (await dataContext.CartItems!
            .Where(x => x.UserId == authenticationService.GetUserId())
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
            .Where(x => x.UserId == authenticationService.GetUserId()).ToListAsync());
    }

    public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
    {
        var dbCartItem = await dataContext.CartItems!
            .FirstOrDefaultAsync(x =>
                x.ProductId == productId &&
                x.ProductTypeId == productTypeId &&
                x.UserId == authenticationService.GetUserId());

        if (dbCartItem is null)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                Sucess = false,
                Message = "Cart item does not exist",
            };
        }

        dataContext.CartItems?.Remove(dbCartItem);
        await dataContext.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true, Sucess = true };
    }

    public async Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItems(List<CartItem> cartItems)
    {
        cartItems.ForEach(cartItem => cartItem.UserId = authenticationService.GetUserId());
        dataContext.CartItems!.AddRange(cartItems);
        await dataContext.SaveChangesAsync();

        return await GetDbCartProducts();
    }

    public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
    {
        var dbCartItem = await dataContext.CartItems!
            .FirstOrDefaultAsync(x =>
                x.ProductId == cartItem.ProductId &&
                x.ProductTypeId == cartItem.ProductTypeId &&
                x.UserId == authenticationService.GetUserId());

        if (dbCartItem is null)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                Sucess = false,
                Message = "Cart item does not exist",
            };
        }

        dbCartItem.Quantity = cartItem.Quantity;
        await dataContext.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true, Sucess = true, };
    }
}
