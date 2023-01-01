using AutoMapper;
using Mango.PaymentApi.Dtos;
using PaymentProcessor;

namespace Mango.PaymentApi.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<PaymentRequestDto, PaymentRequest>();
    }
}