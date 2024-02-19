using BlazorEcommerce.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
    private readonly HttpClient httpClient;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public List<Product> Products { get; set; } = new();

    public event Action ProductsChanged;

    public async Task<ServiceResponse<Product>> GetProduct(int productId)
    {
        var result = await httpClient
            .GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");

        return result!;
    }

    public async Task GetProducts(string? categoryUrl)
    {
        var result = categoryUrl is null ?
            await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
            await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");

        if (result is null || result.Data is null)
        {
            return;
        }

        Products = result.Data;

        ProductsChanged.Invoke();
    }
}
