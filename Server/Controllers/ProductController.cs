using BlazorEcommerce.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly DataContext dataContext;

    public ProductController(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> Get()
    {
        var products = await dataContext.Products.ToListAsync();
        var response = new ServiceResponse<List<Product>>()
        {
            Data = products
        };

        return Ok(response);
    }
}
