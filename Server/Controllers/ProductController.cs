using BlazorEcommerce.Server.Services.ProductService;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
    {
        var result = await productService.CreateProduct(product);

        return Ok(result);
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
    {
        var result = await productService.UpdateProduct(product);

        return Ok(result);
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
    {
        var result = await productService.DeleteProduct(id);

        return Ok(result);
    }

    [HttpGet("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
    {
        var result = await productService.GetAdminProducts();

        return Ok(result);
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

    [HttpGet("search/{phrase}/{page}")]
    public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string phrase, int page)
    {
        var result = await productService.
            SearchProducts(phrase, page);

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
