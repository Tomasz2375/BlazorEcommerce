using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext dataContext;

        public CategoryService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var categories = await dataContext.Categories.ToListAsync();

            return new ServiceResponse<List<Category>>
            {
                Data = categories,
                Sucess = true
            };
        }
    }
}
