using BlazorEcommerce.Server.Services.CartService;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService cartService;

    public CartController(ICartService cartService)
    {
        this.cartService = cartService;
    }

    [HttpPost("products")]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetCartProducts(List<CartItem> cartItems)
    {
        var result = await cartService.GetCartPrductAsync(cartItems);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> StoreCartItems(List<CartItem> cartItems)
    {
        var result = await cartService.StoreCartItems(cartItems);

        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItems)
    {
        var result = await cartService.AddToCart(cartItems);

        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<ActionResult<ServiceResponse<int>>> GetCartItemsCount()
    {
        return await cartService.GetCartItemsCount();
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<CartProductResponseDto>>>> GetDbCartProducts()
    {
        var result = await cartService.GetDbCartProducts();

        return Ok(result);
    }
}
