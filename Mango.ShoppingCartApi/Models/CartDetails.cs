using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.ShoppingCartApi.Models;

public class CartDetails
{
    public Guid Id { get; set; }

    public int Count { get; set; }

    public Guid CartHeaderId { get; set; }

    public Guid ProductId { get; set; }

    [ForeignKey(nameof(CartHeaderId))]
    public CartHeader? CartHeader { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
}