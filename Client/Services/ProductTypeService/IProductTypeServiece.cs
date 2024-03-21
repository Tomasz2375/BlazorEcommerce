namespace BlazorEcommerce.Client.Services.ProductTypeService;

public interface IProductTypeServiece
{
    event Action OnChange;
    public List<ProductType> ProductTypes { get; set; }

    Task GetProductTypes();
    Task AddProductType(ProductType productType);
    Task UpdateProductType(ProductType productType);
    ProductType CreateNewProductType();
}
