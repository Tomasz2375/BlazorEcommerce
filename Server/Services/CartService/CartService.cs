using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.CartService;

public class CartService : ICartService
{
    private readonly DataContext dataContext;

    public CartService(DataContext dataContext)
    {
        this.dataContext = dataContext;
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

            var productVariant = await dataContext.productVariants!
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
}
