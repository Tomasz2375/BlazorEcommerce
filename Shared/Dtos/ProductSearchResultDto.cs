namespace BlazorEcommerce.Shared.Dtos;

public class ProductSearchResultDto
{
    public int Pages { get; set; }
    public int CurrentPage { get; set; }
    public List<Product> Products { get; set; } = new();
}
