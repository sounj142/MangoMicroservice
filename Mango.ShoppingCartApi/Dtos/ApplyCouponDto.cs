namespace Mango.ShoppingCartApi.Dtos;

public class ApplyCouponDto
{
    public string? CouponCode { get; set; }
    public double DiscountAmount { get; set; }
}