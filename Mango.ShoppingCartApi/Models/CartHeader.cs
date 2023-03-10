using System.ComponentModel.DataAnnotations;

namespace Mango.ShoppingCartApi.Models;

public class CartHeader
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? CouponCode { get; set; }

    public double TotalPrice { get; set; }
    public double DiscountAmount { get; set; }
    public double ActualDiscountAmount { get; set; }
    public double FinalPrice { get; set; }

    public IList<CartDetails>? CartDetails { get; set; }
}