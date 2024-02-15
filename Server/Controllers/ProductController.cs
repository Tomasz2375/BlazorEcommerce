using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private static List<Product> products = new()
    {
        new()
        {
            Id = 1,
            Title = "Lord of the rings",
            Description = "LORT kraina książek",
            ImageUrl = "https://krainaksiazek.pl/the_lord_of_the_rings_deluxe_edition-9780544273443.jpg",
            Price = 19.99m,
        },
                new()
        {
            Id = 2,
            Title = "Głębia - skokowiec",
            Description = "Tom 1",
            ImageUrl = "https://static.audioteka.com/pl/images/products/marcin-podlewski/glebia-skokowiec-duze.jpg",
            Price = 9.99m,
        },
                new()
        {
            Id = 3,
            Title = "Glębia - powrót",
            Description = "Tom 2",
            ImageUrl = "https://www.storytel.com/images/640x640/0001986247.jpg",
            Price = 9.99m,
        },
                new()
        {
            Id = 4,
            Title = "Głębia - napór",
            Description = "Tom 3",
            ImageUrl = "https://s.lubimyczytac.pl/upload/books/3872000/3872221/794531-352x500.jpg",
            Price = 9.99m,
        },
                new()
        {
            Id = 5,
            Title = "Głębia - bezkres",
            Description = "Tom 4",
            ImageUrl = "https://fabrykaslow.com.pl/wp/wp-content/uploads/2018/04/PODLEWSKI_Bezkres-x_2D-mala-192x300.jpg",
            Price = 9.99m,
        },
    };

    [HttpGet]
    public async Task<ActionResult<List<Product>>> Get()
    {
        return Ok(products);
    }
}
