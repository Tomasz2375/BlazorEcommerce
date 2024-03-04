using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.CartService;
using System.Security.Claims;

namespace BlazorEcommerce.Server.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly DataContext dataContext;
    private readonly ICartService cartService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public OrderService(
        DataContext dataContext,
        ICartService cartService,
        IHttpContextAccessor httpContextAccessor)
    {
        this.dataContext = dataContext;
        this.cartService = cartService;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<bool>> PlaceOrder()
    {
        var products = (await cartService.GetDbCartProducts()).Data;

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
            UserId = GetUserId(),
            OrderDate = DateTime.UtcNow,
            TotalPrice = totalPrice,
            Items = orderItems
        };
        dataContext.Orders!.Add(order);
        await dataContext.SaveChangesAsync();

        return new ServiceResponse<bool> { Data = true, Sucess = true };
    }

    private int GetUserId() =>
        int.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
}
