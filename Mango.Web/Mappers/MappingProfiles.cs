using AutoMapper;
using Mango.Web.Dtos;

namespace Mango.Web.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ProductDto, ProductUpdateDto>();
        CreateMap<ProductDto, ProductDetailsDto>();
    }
}