namespace BlazorEcommerce.Server.Services.ProductTypeServices;

public interface IProductTypeService
{
    Task<ServiceResponse<List<ProductType>>> GetProductTypes();
}
