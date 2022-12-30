namespace Mango.Web.Dtos;

public class ApplyCouponDto
{
    public string UserId { get; set; } = string.Empty;
    public string? CouponCode { get; set; }
    public double DiscountAmount { get; set; }
}