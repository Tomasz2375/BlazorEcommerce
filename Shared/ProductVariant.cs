using BlazorEcommerce.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorEcommerce;

public class ProductVariant
{
    public int ProductId { get; set; }
    public int ProductTypeId { get; set; }

    [JsonIgnore]
    public Product? Product { get; set; }
    public ProductType? ProductType { get; set; }

    public bool Visible { get; set; } = true;
    public bool Deleted { get; set; }
    [NotMapped]
    public bool Editing { get; set; }
    [NotMapped]
    public bool IsNew { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal OriginalPrice { get; set; }
}
