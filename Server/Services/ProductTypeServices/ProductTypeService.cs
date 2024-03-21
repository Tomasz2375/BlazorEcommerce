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

    public async Task<ServiceResponse<List<ProductType>>> AddProductType(ProductType productType)
    {
        dataContext.ProductTypes!.Add(productType);

        await dataContext.SaveChangesAsync();

        return await GetProductTypes();
    }

    public async Task<ServiceResponse<List<ProductType>>> GetProductTypes()
    {
        var productTypes = await dataContext.ProductTypes!.ToListAsync();

        return new ServiceResponse<List<ProductType>>
        {
            Data = productTypes,
            Sucess = true,
        };
    }

    public async Task<ServiceResponse<List<ProductType>>> UpdateProductType(ProductType productType)
    {
        var dbProductType = await dataContext.ProductTypes.FindAsync(productType.Id);
        if (dbProductType is null)
        {
            return new ServiceResponse<List<ProductType>>()
            {
                Sucess = false,
                Message = "Product Type not found",
            };
        }

        dbProductType.Name = productType.Name;

        await dataContext.SaveChangesAsync();

        return await GetProductTypes();
    }
}
