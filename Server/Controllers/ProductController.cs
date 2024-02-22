using BlazorEcommerce.Server.Services.ProductService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;

    public ProductController(IProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
    {
        var result = await productService.GetProductsAsync();

        return Ok(result);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
    {
        var result = await productService.GetProductAsync(productId);

        return Ok(result);
    }

    [HttpGet("category/{categoryUrl}")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProuctsByCategory(string categoryUrl)
    {
        var result = await productService.
            GetProductsByCategoryAsync(categoryUrl);

        return Ok(result);
    }

    [HttpGet("search/{phrase}")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> SearchProducts(string phrase)
    {
        var result = await productService.
            SearchProducts(phrase);

        return Ok(result);
    }

    [HttpGet("searchsuggestion/{phrase}")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> SearchSuggestions(string phrase)
    {
        var result = await productService
            .GetProductSearchSuggesions(phrase);

        return Ok(result);
    }

    [HttpGet("featured")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
    {
        var result = await productService
            .GetFeaturedProducts();

        return Ok(result);
    }
}
