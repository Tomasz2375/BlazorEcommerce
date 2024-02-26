using BlazorEcommerce.Shared.Dtos;

namespace BlazorEcommerce.Server.Services.ProductService;

public interface IProductService
{
    Task<ServiceResponse<List<Product>>> GetProductsAsync();
    Task<ServiceResponse<Product>> GetProductAsync(int productId);
    Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
    Task<ServiceResponse<ProductSearchResultDto>> SearchProducts(string phrase, int page);
    Task<ServiceResponse<List<string>>> GetProductSearchSuggesions(string phrase);
    Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
}
