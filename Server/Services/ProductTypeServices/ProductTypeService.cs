using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductTypeServices;

public class ProductTypeService : IProductTypeService
{
    private readonly DataContext dataContext;

    public ProductTypeService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
    {
        var productTypes = await dataContext.ProductTypes.ToListAsync();

        return new ServiceResponse<List<ProductType>>
        {
            Data = productTypes,
            Sucess = true,
        };
    }
}
