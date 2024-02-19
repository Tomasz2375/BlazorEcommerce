using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.CategoryServices;

public interface ICategoryService
{
    Task<ServiceResponse<List<Category>>> GetCategories();
}
