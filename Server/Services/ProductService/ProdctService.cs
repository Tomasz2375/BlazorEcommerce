using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductService;

public class ProdctService : IProductService
{
    private readonly DataContext dataContext;

    public ProdctService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
    {
        var response = new ServiceResponse<Product>();
        var product = await dataContext
            .Products!
            .Include(x => x.Variants)
            .ThenInclude(x => x.ProductType)
            .FirstOrDefaultAsync(x => x.Id == productId);

        if(product is null)
        {
            response.Sucess = false;
            response.Message = "Sorry, but this product does not exist.";
        }
        else
        {
            response.Sucess = true;
            response.Data = product;
        }

        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
    {
        var response = new ServiceResponse<List<Product>>
        {
            Data = await dataContext.Set<Product>()
            .Include(x => x.Variants)
            .ToListAsync(),
        };

        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
    {
        var response = new ServiceResponse<List<Product>>
        {
            Data = await dataContext.Products!
                .Where(x => x.Category!.Url.ToLower() == categoryUrl.ToLower())
                .Include(x => x.Variants)
                .ToListAsync()
        };

        return response;
    }

    public async Task<ServiceResponse<List<Product>>> SearchProducts(string phrase)
    {
        var response = new ServiceResponse<List<Product>>
        {
            Data = await dataContext
            .Products!
            .Include(x => x.Variants)
            .Where(x =>
                x.Title.ToLower().Contains(phrase.ToLower()) ||
                x.Description.ToLower().Contains(phrase.ToLower())
            ).ToListAsync()
        };

        return response;
    }
}
