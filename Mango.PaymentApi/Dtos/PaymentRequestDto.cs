namespace Mango.PaymentApi.Dtos;

public class PaymentRequestDto
{
    public Guid OrderId { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CardNumber { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonthYear { get; set; }

    public decimal Amount { get; set; }

    public string? Email { get; set; }
}