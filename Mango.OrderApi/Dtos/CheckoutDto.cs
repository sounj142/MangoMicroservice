namespace Mango.OrderApi.Dtos;

public class CheckoutDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime PickupDateTime { get; set; }
    public int TimeZoneOffset { get; set; }

    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? CardNumber { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonthYear { get; set; }

    // all values bellow are fore comparison only. We don't save these into database due to
    // they're not safe. We need to re-caculate, and after that, if the new calcualted
    // values are diferent, we need to show error

    public string? CouponCode { get; set; }
    public double TotalPrice { get; set; }
    public double DiscountAmount { get; set; }
    public double ActualDiscountAmount { get; set; }
    public double FinalPrice { get; set; }

    public CartHeaderDto? Cart { get; set; }
}