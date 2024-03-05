using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.AuthenticationServices;
using BlazorEcommerce.Server.Services.CartService;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly DataContext dataContext;
    private readonly ICartService cartService;
    private readonly IAuthenticationService authenticationService;

    public OrderService(
        DataContext dataContext,
        ICartService cartService,
        IAuthenticationService authenticationService)
    {
        this.dataContext = dataContext;
        this.cartService = cartService;
        this.authenticationService = authenticationService;
    }

    public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
    {
        var response = new ServiceResponse<List<OrderOverviewResponse>>();
        var orders = await dataContext.Orders!
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .Where(x => x.UserId == authenticationService.GetUserId())
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync();

        List<OrderOverviewResponse> orderResponse = new();
        orders.ForEach(x => orderResponse.Add(new OrderOverviewResponse()
        {
            Id = x.Id,
            OrderDate = x.OrderDate,
            TotalPrice = x.TotalPrice,
            Product = x.OrderItems.Count > 1 ?
                $"{x.OrderItems.First().Product.Title} and " +
                $"{x.OrderItems.Count - 1} more..." :
                x.OrderItems.First().Product.Title,
            ProductImageUrl = x.OrderItems.First().Product.ImageUrl,
        }));
        response.Data = orderResponse;

        return response;
    }

    public async Task<ServiceResponse<bool>> PlaceOrder()
    {
        var products = (await cartService.GetDbCartProducts()).Data;
        var userId = authenticationService.GetUserId();
        decimal totalPrice = 0;
        products!.ForEach(product => totalPrice += product.Price * product.Quantity);

        var orderItems = new List<OrderItem>();
        products.ForEach(product => orderItems.Add(new OrderItem()
        {
            ProductId = product.ProductId,
            ProductTypeId = product.ProductTypeId,
            Quantity = product.Quantity,
            TotalPrice = product.Price * product.Quantity,
        }));

        var order = new Order()
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            TotalPrice = totalPrice,
            OrderItems = orderItems
        };
        dataContext.Orders!.Add(order);
        dataContext.CartItems!.RemoveRange(dataContext.CartItems
            .Where(x => x.UserId == userId));

        await dataContext.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true, Sucess = true };
    }
}
