using BlazorEcommerce.Shared;
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
    public List<Category> AdminCategories { get; set; } = new();

    public event Action OnChange;

    public async Task AddCategory(Category category)
    {
        var response = await httpClient.PostAsJsonAsync("api/Category/admin", category);

        AdminCategories = 
            (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>())!
            .Data!;

        await GetCategories();
        OnChange.Invoke();
    }

    public Category CreateCategory(Category category)
    {
        var newCategory = new Category
        {
            IsNew = true,
            Editing = true,
        };

        OnChange.Invoke();

        return newCategory;
    }

    public async Task DeleteCategory(int id)
    {
        var response = await httpClient.DeleteAsync($"api/Category/admin/{id}");

        AdminCategories =
            (await response.Content
            .ReadFromJsonAsync<ServiceResponse<List<Category>>>())!
            .Data!;

        await GetCategories();
        OnChange.Invoke();
    }

    public async Task GetAdminCategories()
    {
        var response = await httpClient
            .GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category/admin");
        if (response is null || response.Data is null)
        {
            return;
        }

        AdminCategories = response.Data;
    }

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

    public async Task UpdateCategory(Category category)
    {
        var response = await httpClient.PutAsJsonAsync("api/Category/admin", category);

        AdminCategories =
            (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Category>>>())!
            .Data!;

        await GetCategories();
        OnChange.Invoke();
    }
}
