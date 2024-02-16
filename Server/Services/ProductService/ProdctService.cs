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

    public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
    {
        var response = new ServiceResponse<List<Product>>
        {
            Data = await dataContext.Set<Product>().ToListAsync(),
        };

        return response;
    }
}
