using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductTypeService;

public class ProductTypeServiece : IProductTypeServiece
{
    private readonly HttpClient httpClient;

    public ProductTypeServiece(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public List<ProductType> ProductTypes { get; set; }
    public event Action OnChange;

    public async Task GetProductTypes()
    {
        var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/productType");

        ProductTypes = result!.Data!;
    }
}
