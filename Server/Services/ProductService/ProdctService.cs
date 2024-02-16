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
        var product = await dataContext.Products.FindAsync(productId);

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
            Data = await dataContext.Set<Product>().ToListAsync(),
        };

        return response;
    }
}
