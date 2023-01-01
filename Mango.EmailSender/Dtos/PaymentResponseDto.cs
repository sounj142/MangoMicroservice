namespace Mango.EmailSender.Dtos;

public class PaymentResponseDto
{
    public Guid OrderId { get; set; }

    public bool Paid { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}