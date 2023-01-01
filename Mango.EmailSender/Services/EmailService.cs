using AutoMapper;
using Mango.EmailSender.Dtos;
using Mango.EmailSender.Models;
using Mango.EmailSender.Repositories;

namespace Mango.EmailSender.Services;

public class EmailService : IEmailService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EmailService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task SendEmail(PaymentResponseDto dto)
    {
        var email = new Email
        {
            Id = Guid.NewGuid(),
            SendDate = DateTime.UtcNow,
            To = dto.Email,
            Body = $"Order {dto.OrderId} has been created successfully."
        };
        _dbContext.Emails.Add(email);

        await _dbContext.SaveChangesAsync();
    }
}