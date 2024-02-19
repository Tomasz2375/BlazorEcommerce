using BlazorEcommerce.Server.Services.CategoryServices;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories()
    {
        var result = await categoryService.GetCategories();

        return Ok(result);
    }
}
