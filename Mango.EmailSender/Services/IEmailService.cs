using Mango.EmailSender.Dtos;

namespace Mango.EmailSender.Services;

public interface IEmailService
{
    Task SendEmail(PaymentResponseDto dto);
}