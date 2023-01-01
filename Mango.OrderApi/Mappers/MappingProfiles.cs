using AutoMapper;
using Mango.OrderApi.Dtos;
using Mango.OrderApi.Models;

namespace Mango.OrderApi.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CartDetailsDto, OrderDetails>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Product, dest));
        CreateMap<ProductDto, OrderDetails>()
            .ForMember(x => x.Id, opt => opt.Ignore());

        CreateMap<CheckoutDto, OrderHeader>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .AfterMap((src, dest, context) => context.Mapper.Map(src.Cart, dest));
        CreateMap<CartHeaderDto, OrderHeader>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.OrderDetails, opt => opt.MapFrom(s => s.CartDetails));

        CreateMap<OrderHeader, PaymentRequestDto>()
            .ForMember(x => x.OrderId, opt => opt.MapFrom(s => s.Id))
            .ForMember(x => x.Amount, opt => opt.MapFrom(s => s.FinalPrice));
    }
}