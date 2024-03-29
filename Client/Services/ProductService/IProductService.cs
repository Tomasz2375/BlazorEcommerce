﻿using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductService
{
    event Action ProductsChanged;
    string Message { get; set; }
    int CurrentPage { get; set; }
    int PageCount { get; set; }
    string LastSearchText { get; set; }

    List<Product> Products { get; set; }
    List<Product> AdminProducts { get; set; }
    Task GetProducts(string? categoryUrl = null);
    Task<ServiceResponse<Product>> GetProduct(int productId);
    Task SearchProducts(string phrase, int page);
    Task<List<string>> GetProductSearchSuggestion(string phrase);
    Task GetAdminProducts();

    Task<Product> CreateProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task DeleteProduct(Product product);
}
