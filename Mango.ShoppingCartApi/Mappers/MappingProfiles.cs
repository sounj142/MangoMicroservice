using AutoMapper;
using Mango.ShoppingCartApi.Dtos;
using Mango.ShoppingCartApi.Models;

namespace Mango.ShoppingCartApi.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<CartHeaderDto, CartHeader>().ReverseMap();
        CreateMap<CartDetailsDto, CartDetails>().ReverseMap();
    }
}