using System.ComponentModel.DataAnnotations;

namespace Mango.OrderApi.Models;

public class OrderHeader
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? CouponCode { get; set; }

    public double TotalPrice { get; set; }
    public double DiscountAmount { get; set; }
    public double ActualDiscountAmount { get; set; }
    public double FinalPrice { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime PickupDateTime { get; set; }
    public int TimeZoneOffset { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? CardNumber { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonthYear { get; set; }

    public DateTime OrderTime { get; set; }
    public bool PaymentStatus { get; set; }

    public IList<OrderDetails>? OrderDetails { get; set; }
}