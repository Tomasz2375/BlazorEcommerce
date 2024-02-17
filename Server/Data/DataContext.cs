using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category()
            {
                Id = 1,
                Name = "Books",
                Url = "books"
            },
            new Category()
            {
                Id = 2,
                Name = "Movies",
                Url = "movies"
            },
            new Category()
            {
                Id = 3,
                Name = "Games",
                Url = "games"
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product()
            {
                Id = 1,
                Title = "Lord of the rings",
                Description = "LORT kraina książek",
                ImageUrl = "https://krainaksiazek.pl/the_lord_of_the_rings_deluxe_edition-9780544273443.jpg",
                Price = 19.99m,
                CategoryId = 1,
            },
            new Product()
            {
                Id = 2,
                Title = "Głębia - skokowiec",
                Description = "Tom 1",
                ImageUrl = "https://static.audioteka.com/pl/images/products/marcin-podlewski/glebia-skokowiec-duze.jpg",
                Price = 9.99m,
                CategoryId = 1,
            },
            new Product()
            {
                Id = 3,
                Title = "Glębia - powrót",
                Description = "Tom 2",
                ImageUrl = "https://www.storytel.com/images/640x640/0001986247.jpg",
                Price = 9.99m,
                CategoryId = 1,
            },
            new Product()
            {
                Id = 4,
                Title = "Głębia - napór",
                Description = "Tom 3",
                ImageUrl = "https://s.lubimyczytac.pl/upload/books/3872000/3872221/794531-352x500.jpg",
                Price = 9.99m,
                CategoryId = 1,
            },
            new Product()
            {
                Id = 5,
                Title = "Głębia - bezkres",
                Description = "Tom 4",
                ImageUrl = "https://fabrykaslow.com.pl/wp/wp-content/uploads/2018/04/PODLEWSKI_Bezkres-x_2D-mala-192x300.jpg",
                Price = 9.99m,
                CategoryId = 1,
            }
        );
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
