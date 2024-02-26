using BlazorEcommerce.Shared;
using BlazorEcommerce.Shared.Dtos;
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
    public string Message { get; set; } = "Loading products...";
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; }
    public string LastSearchText { get; set; } = string.Empty;

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
            await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") :
            await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");

        if (result is null || result.Data is null)
        {
            return;
        }
        Products = result.Data;

        CurrentPage = 1;
        PageCount = 0;
        if (Products.Count == 0)
        {
            Message = "No products found";
        }

        ProductsChanged.Invoke();
    }

    public async Task<List<string>> GetProductSearchSuggestion(string phrase)
    {
        var result = await httpClient
            .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestion/{phrase}");

        if (result is null || result.Data is null)
        {
            return new List<string>();
        }

        return result.Data!;
    }

    public async Task SearchProducts(string phrase, int page)
    {
        var result = await httpClient
            .GetFromJsonAsync<ServiceResponse<ProductSearchResultDto>>($"api/product/search/{phrase}/{page}");

        if (result is null || result.Data is null)
        {
            return;
        }

        Products = result.Data.Products;
        CurrentPage = result.Data.CurrentPage;
        PageCount = result.Data.Pages;

        if (Products.Count == 0)
        {
            Message = "No products found.";
        }

        ProductsChanged.Invoke();
    }
}
