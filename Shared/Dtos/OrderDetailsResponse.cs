namespace BlazorEcommerce.Shared.Dtos;

public class OrderDetailsResponse
{
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderDetailsPropductResponse> Products { get; set; } = new();
}
