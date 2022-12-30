using System.ComponentModel.DataAnnotations;

namespace Mango.CouponApi.Models;

public class Coupon
{
    [Key]
    [MaxLength(200)]
    public string CouponCode { get; set; } = string.Empty;

    public double DiscountAmount { get; set; }
}