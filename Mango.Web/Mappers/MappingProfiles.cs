using AutoMapper;
using Commons.Dtos;

namespace Mango.Web.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ProductDto, ProductUpdateDto>();
    }
}