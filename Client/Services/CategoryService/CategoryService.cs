using System.Net.Http.Json;

namespace BlazorEcommerce.Client.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly HttpClient httpClient;

    public CategoryService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public List<Category> Categories { get; set; } = new();

    public async Task GetCategories()
    {
        var response = await httpClient
            .GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");
        if (response is null || response.Data is null)
        {
            return;
        }

        Categories = response.Data;

    }
}
