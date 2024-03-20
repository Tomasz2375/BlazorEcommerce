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
}
