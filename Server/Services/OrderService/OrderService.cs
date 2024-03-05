using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.AuthenticationServices;
using BlazorEcommerce.Server.Services.CartService;
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
            Items = orderItems
        };
        dataContext.Orders!.Add(order);
        dataContext.CartItems!.RemoveRange(dataContext.CartItems
            .Where(x => x.UserId == userId));

        await dataContext.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true, Sucess = true };
    }
}
