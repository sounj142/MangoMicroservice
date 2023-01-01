namespace Mango.PaymentApi.Dtos;

public class PaymentResponseDto
{
    public Guid OrderId { get; set; }

    public bool Paid { get; set; }
}