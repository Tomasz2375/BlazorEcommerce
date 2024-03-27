using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductService;

public class ProdctService : IProductService
{
    private readonly DataContext dataContext;
    private readonly IHttpContextAccessor httpContextAccessor;

    public ProdctService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        this.dataContext = dataContext;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
    {
        var response = new ServiceResponse<Product>();
        Product? product;

        if (httpContextAccessor.HttpContext!.User.IsInRole("Admin"))
        {
            product = await dataContext
                .Products!
                .Include(x => x.Variants.Where(v => !v.Deleted))
                .ThenInclude(x => x.ProductType)
                .FirstOrDefaultAsync(x => x.Id == productId && !x.Deleted)!;
        }
        else
        {
            product = await dataContext
                .Products!
                .Include(x => x.Variants.Where(v => !v.Deleted && x.Visible))
                .ThenInclude(x => x.ProductType)
                .FirstOrDefaultAsync(x => x.Id == productId && !x.Deleted && x.Visible)!;
        }


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
            .Where(x => x.Visible && !x.Deleted)
            .Include(x => x.Variants
                .Where(y => y.Visible && !y.Deleted))
            .ToListAsync(),
        };

        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
    {
        var response = new ServiceResponse<List<Product>>
        {
            Data = await dataContext.Products!
                .Where(x => x.Category!.Url.ToLower() == categoryUrl.ToLower() &&
                    x.Visible && !x.Deleted)
                .Include(x => x.Variants
                    .Where(y => y.Visible && !y.Deleted))
                .ToListAsync()
        };

        return response;
    }

    public async Task<ServiceResponse<ProductSearchResultDto>> SearchProducts(string phrase, int page)
    {
        var pageResults = 2;
        var pageCount = MathF.Ceiling((await getProductBySearchPhrase(phrase)).Count / (float)pageResults);
        var products = await dataContext
            .Products!
            .Include(x => x.Variants)
            .Where(x =>
                x.Title.ToLower().Contains(phrase.ToLower()) ||
                x.Description.ToLower().Contains(phrase.ToLower()) &&
                x.Visible &&
                !x.Deleted)
            .Skip((page - 1) * pageResults)
            .Take(pageResults)
            .ToListAsync();

        var response = new ServiceResponse<ProductSearchResultDto>
        {
            Data = new ProductSearchResultDto
            {
                Products = products,
                CurrentPage = page,
                Pages = (int)pageCount,
            }
        };

        return response;

    }

    public async Task<ServiceResponse<Product>> CreateProduct(Product product)
    {
        foreach (var variant in product.Variants)
        {
            variant.ProductType = null!;
        }

        dataContext.Products!.Add(product);
        await dataContext.SaveChangesAsync();

        return new ServiceResponse<Product>()
        {
            Data = product,
        };
    }

    public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
    {
        var dbProduct = await dataContext.Products!.FirstOrDefaultAsync(x => x.Id == product.Id);

        if (dbProduct is null)
        {
            return new ServiceResponse<Product>
            {
                Sucess = false,
                Message = "Product not found",
            };
        }

        dbProduct.Title = product.Title;
        dbProduct.Description = product.Description;
        dbProduct.ImageUrl = product.ImageUrl;
        dbProduct.CategoryId = product.CategoryId;
        dbProduct.Visible = product.Visible;
        dbProduct.Featured = product.Featured;

        foreach (var variant in product.Variants)
        {
            var dbVariant = await dataContext.ProductVariants!
                .SingleOrDefaultAsync(x =>
                    x.ProductId == variant.ProductId &&
                    x.ProductTypeId == variant.ProductTypeId);
            if (dbVariant is null)
            {
                variant.ProductType = null!;
                dataContext.ProductVariants!.Add(variant);
            }
            else
            {
                dbVariant.ProductTypeId = variant.ProductTypeId;
                dbVariant.Price = variant.Price;
                dbVariant.OriginalPrice = variant.OriginalPrice;
                dbVariant.Visible = variant.Visible;
                dbVariant.Deleted = variant.Deleted;
            }
        }

        await dataContext.SaveChangesAsync();

        return new ServiceResponse<Product>
        {
            Data = product,
            Sucess = true,
        };
    }

    public async Task<ServiceResponse<bool>> DeleteProduct(int productId)
    {
        var dbProduct = await dataContext.Products!.FirstOrDefaultAsync(x => x.Id == productId);
        if (dbProduct is null)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                Sucess = false,
                Message = "Product not found",
            };
        }

        dbProduct.Deleted = true;
        await dataContext.SaveChangesAsync();

        return new ServiceResponse<bool>()
        {
            Data = true,
            Sucess = true,
        };
    }

    public async Task<ServiceResponse<List<string>>> GetProductSearchSuggesions(string phrase)
    {
        var proudcts = await getProductBySearchPhrase(phrase);

        List<string> result = new();

        foreach (var product in proudcts)
        {
            if (product.Title.Contains(phrase, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(product.Title);
            }

            if (product.Description is not null)
            {
                var punctuation = product
                    .Description
                    .Where(char.IsPunctuation)
                    .Distinct()
                    .ToArray();

                var words = product.Description.Split()
                    .Select(x => x.Trim(punctuation));

                foreach (var word in words)
                {
                    if (word.Contains(phrase, StringComparison.OrdinalIgnoreCase) &&
                        !result.Contains(word))
                    {
                        result.Add(word);
                    }
                }
            }
        }

        return new ServiceResponse<List<string>> { Data = result };
    }

    public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
    {
        var response = new ServiceResponse<List<Product>>
        {
            Data = await dataContext.Set<Product>()
                .Where(x => !x.Deleted)
                .Include(x => x.Variants
                    .Where(y => !y.Deleted))
                .ThenInclude(x => x.ProductType)
                .ToListAsync(),
        };

        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
    {
        var response = new ServiceResponse<List<Product>>()
        {
            Data = await dataContext
            .Products!
            .Include(x => x.Variants
                .Where(y => y.Visible && !y.Deleted))
            .Where(x => x.Featured && x.Visible && !x.Deleted)
            .ToListAsync(),
        };

        return response;
    }

    private async Task<List<Product>> getProductBySearchPhrase(string phrase)
    {
        return await dataContext
                    .Products!
                    .Where(x =>
                        x.Title.ToLower().Contains(phrase.ToLower()) ||
                        x.Description.ToLower().Contains(phrase.ToLower()) &&
                        x.Visible &&
                        !x.Deleted)
                    .Include(x => x.Variants)
                    .ToListAsync();
    }

}
