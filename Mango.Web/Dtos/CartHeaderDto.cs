namespace Mango.Web.Dtos;

public class CartHeaderDto
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string? CouponCode { get; set; }

    public double TotalPrice { get; set; }
    public double DiscountAmount { get; set; }
    public double ActualDiscountAmount { get; set; }
    public double FinalPrice { get; set; }

    // first, last name : OrderDto

    public IList<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
}