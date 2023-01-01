using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Dtos;

public class CheckoutDto
{
    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    public DateTime PickupDateTime { get; set; }
    public int TimeZoneOffset { get; set; }

    [Required]
    public string? Phone { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string? CardNumber { get; set; }

    [Required]
    public string? CVV { get; set; }

    [Required]
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