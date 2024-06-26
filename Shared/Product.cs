﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared;

public class Product
{
    public int Id { get; set; }
    public int CategoryId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool Featured { get; set; }
    public bool Visible { get; set; } = true;
    public bool Deleted { get; set; }
    [NotMapped]
    public bool Editing { get; set; }
    [NotMapped]
    public bool IsNew { get; set; }

    public Category? Category { get; set; }

    public List<ProductVariant> Variants { get; set; } = new();
    public List<Image> Images { get; set; } = new();
}
