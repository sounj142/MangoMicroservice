using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.OrderApi.Models;

public class OrderDetails
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public int Count { get; set; }

    public Guid OrderHeaderId { get; set; }

    public Guid ProductId { get; set; }

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

    [ForeignKey(nameof(OrderHeaderId))]
    public OrderHeader? OrderHeader { get; set; }
}