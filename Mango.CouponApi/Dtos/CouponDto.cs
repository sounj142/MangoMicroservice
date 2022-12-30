namespace Mango.CouponApi.Dtos;

public class CouponDto
{
    public string CouponCode { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }
}