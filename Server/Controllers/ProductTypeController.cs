using BlazorEcommerce.Server.Services.ProductTypeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ProductTypeController : ControllerBase
{
    private readonly IProductTypeService productTypeService;

    public ProductTypeController(IProductTypeService productTypeService)
    {
        this.productTypeService = productTypeService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<ProductType>>>> GetProductTypes()
    {
        var response = await productTypeService.GetProductTypes();

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<ProductType>>>> AddProductTypes(ProductType productType)
    {
        var response = await productTypeService.AddProductType(productType);

        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<List<ProductType>>>> UpdateProductTypes(ProductType productType)
    {
        var response = await productTypeService.UpdateProductType(productType);

        return Ok(response);
    }
}
