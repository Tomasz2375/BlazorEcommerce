using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.ProductTypeService;

public class ProductTypeServiece : IProductTypeServiece
{
    private readonly HttpClient httpClient;

    public ProductTypeServiece(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public List<ProductType> ProductTypes { get; set; } = new();
    public event Action OnChange;

    public async Task AddProductType(ProductType productType)
    {
        var response = await httpClient.PostAsJsonAsync("api/productType", productType);

        ProductTypes = (await response
            .Content!
            .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>())!.Data!;
        OnChange.Invoke();
    }

    public ProductType CreateNewProductType()
    {
        var newProductType = new ProductType()
        {
            IsNew = true,
            Editing = true,
        };

        ProductTypes.Add(newProductType);
        OnChange.Invoke();
        return newProductType;
    }

    public async Task GetProductTypes()
    {
        var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/productType");

        ProductTypes = result!.Data!;
    }

    public async Task UpdateProductType(ProductType productType)
    {
        var response = await httpClient.PutAsJsonAsync("api/productType", productType);

        ProductTypes = (await response
            .Content!
            .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>())!.Data!;

        OnChange?.Invoke();
    }
}
