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

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;

            dataContext.Categories!.Add(category);
            await dataContext.SaveChangesAsync();

            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategory(int id)
        {
            var category = await getCategoryById(id);

            if (category is null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Sucess = false,
                    Message = "Category not found.",
                };
            }

            category.Deleted = true;
            await dataContext.SaveChangesAsync();

            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
            var categories = await dataContext.Categories!
                .Where(x => !x.Deleted)
                .ToListAsync();

            return new ServiceResponse<List<Category>>
            {
                Data = categories,
                Sucess = true
            };
        }

        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var categories = await dataContext.Categories!
                .Where(x => !x.Deleted && x.Visible)
                .ToListAsync();

            return new ServiceResponse<List<Category>>
            {
                Data = categories,
                Sucess = true
            };
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategory(Category category)
        {
            var dbCategory = await getCategoryById(category.Id);

            if (dbCategory is null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Sucess = false,
                    Message = "Category not found.",
                };
            }

            dbCategory.Name = category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await dataContext.SaveChangesAsync();

            return await GetAdminCategories();
        }

        private async Task<Category?> getCategoryById(int id)
        {
            return await dataContext.Categories!
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
