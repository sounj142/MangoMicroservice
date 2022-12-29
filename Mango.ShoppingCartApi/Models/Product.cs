using System.ComponentModel.DataAnnotations;

namespace Mango.ShoppingCartApi.Models;

public class Product
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public double Price { get; set; }

    public string? Description { get; set; }

    [MaxLength(1000)]
    public string? ImageUrl { get; set; }

    public Guid CategoryId { get; set; }

    [MaxLength(1000)]
    public string CategoryName { get; set; } = string.Empty;

    public IList<CartDetails>? CartDetails { get; set; }
}