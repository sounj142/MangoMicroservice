using System.ComponentModel.DataAnnotations;

namespace Mango.ProductAPI.Models;

public class Product
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000)]
    public double Price { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public Guid CategoryId { get; set; }

    public Category? Category { get; set; }
}