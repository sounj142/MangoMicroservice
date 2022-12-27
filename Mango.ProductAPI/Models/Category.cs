using System.ComponentModel.DataAnnotations;

namespace Mango.ProductAPI.Models;

public class Category
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Name { get; set; } = string.Empty;

    public IList<Product>? Products { get; set; }
}