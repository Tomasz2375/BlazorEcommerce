using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Client.Services.OrderService;

public interface IOrderService
{
    Task PlaceOrder();
    Task<List<OrderOverviewResponse>> GetOrders();
}
