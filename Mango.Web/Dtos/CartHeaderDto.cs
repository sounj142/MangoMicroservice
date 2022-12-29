namespace Mango.Web.Dtos;

public class CartHeaderDto
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string? CouponCode { get; set; }

    public decimal TotalCoupon { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal FinalPrice { get; set; }

    public IList<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
}