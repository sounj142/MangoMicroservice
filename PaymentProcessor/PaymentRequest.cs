namespace PaymentProcessor;

public class PaymentRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CardNumber { get; set; }
    public string? CVV { get; set; }
    public string? ExpiryMonthYear { get; set; }

    public decimal Amount { get; set; }
}